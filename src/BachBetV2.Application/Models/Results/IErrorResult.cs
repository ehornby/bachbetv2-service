namespace BachBetV2.Application.Models.Results
{
    internal interface IErrorResult
    {
        IReadOnlyCollection<Error> Errors { get; }
        string Message { get; }
    }
}