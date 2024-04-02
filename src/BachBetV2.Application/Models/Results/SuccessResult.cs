namespace BachBetV2.Application.Models.Results
{
    public sealed class SuccessResult : Result
    {
        public SuccessResult()
        {
            Success = true;
        }
    }

    public sealed class SuccessResult<T> : Result<T>
    {
        public SuccessResult(T data) : base(data)
        {
            Success = true;
        }
    }
}
