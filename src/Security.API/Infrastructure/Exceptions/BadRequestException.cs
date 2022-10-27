using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace N5.Challenge.Services.Security.API.Infrastructure.Exceptions
{
    public abstract class BadRequestException : ApplicationException
    {
        protected BadRequestException(string message)
            : base("Bad Request", message)
        { }

        [ExcludeFromCodeCoverage]
        protected BadRequestException(SerializationInfo info, StreamingContext context)
            : base(info, context) 
        { }
    }
}