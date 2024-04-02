namespace BachBetV2.Application.Models.Results
{
    public sealed class BadRequestErrorResult : ErrorResult
    {
        public BadRequestErrorResult(string message) : base(message)
        {

        }
    }
}
