using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace N5.Challenge.Services.Security.API.Infrastructure.Exceptions
{
    public abstract class ApplicationException : Exception
    {
        public string? Title { get; }

        protected ApplicationException(string title, string message)
            : base(message)
            => Title = title;

        [ExcludeFromCodeCoverage]
        protected ApplicationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}