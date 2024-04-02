namespace BachBetV2.Application.Models.Results
{
    public class NotFoundResult<T> : ErrorResult<T>
    {
        public NotFoundResult(string message) : base(message, Array.Empty<Error>())
        {
        }
    }
    public class NotFoundResult : ErrorResult
    {
        public NotFoundResult(string message) : base(message, Array.Empty<Error>())
        {
        }
    }

}
