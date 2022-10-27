namespace N5.Challenge.Services.Security.Domain.Exceptions
{
    public class SecurityDomainException : Exception
    {
        private int errorId;

        public int ErrorId => errorId;

        public SecurityDomainException()
        { }

        public SecurityDomainException(string message)
            : base(message)
        { }

        public SecurityDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }

        public SecurityDomainException(string message, int errorId)
            : base(message)
        { 
            this.errorId = errorId;
        }
    }
}