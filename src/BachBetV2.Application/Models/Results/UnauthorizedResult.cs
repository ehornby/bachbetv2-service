namespace BachBetV2.Application.Models.Results
{
    public sealed class UnauthorizedResult<T> : ErrorResult<T>
    {
        public UnauthorizedResult(string message) : base(message, Array.Empty<Error>())
        {
        }
    }
}
