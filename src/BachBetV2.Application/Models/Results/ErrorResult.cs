namespace BachBetV2.Application.Models.Results
{
    public class ErrorResult : Result, IErrorResult
    {
        public string Message { get; }
        public IReadOnlyCollection<Error> Errors { get; }

        public ErrorResult(string message) : this(message, Array.Empty<Error>())
        {
        }

        public ErrorResult(string message, IReadOnlyCollection<Error> errors)
        {
            Message = message;
            Success = false;
            Errors = errors ?? Array.Empty<Error>();
        }
    }

    public class ErrorResult<T> : Result<T>, IErrorResult
    {
        public string Message { get; }
        public IReadOnlyCollection<Error> Errors { get; }

        public ErrorResult(string message) : this(message, Array.Empty<Error>())
        {
        }

        public ErrorResult(string message, IReadOnlyCollection<Error> errors) : base(default!)
        {
            Message = message;
            Success = false;
            Errors = errors ?? Array.Empty<Error>();
        }
    }
}
