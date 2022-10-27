namespace N5.Challenge.Services.Security.API.Responses
{
    /// <summary>
    /// Error model returned to user.
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Get the error message.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Get the error status.
        /// </summary>
        public int Status { get; }

        /// <summary>
        /// Get the error title.
        /// </summary>
        public string? Title { get; }

        /// <summary>
        /// Get dictionary with error details.
        /// </summary>
        public IReadOnlyDictionary<string, string[]>? Errors  { get; }

        /// <summary>
        /// Create a new error response to display to the user.
        /// </summary>
        /// <param name="message">Message to display.</param>
        /// <param name="status">Status code.</param>
        /// <param name="title">Title to display.</param>
        /// <param name="errors">Dictionary of errors.</param>
        public ErrorResponse(string message, int status, string? title,
            IReadOnlyDictionary<string, string[]>? errors)
        {
            Message = message;
            Status = status;
            Title = title;
            Errors = errors;
        }
    }
}