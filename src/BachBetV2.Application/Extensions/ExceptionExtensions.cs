using BachBetV2.Application.Models.Results;

namespace BachBetV2.Application.Extensions
{
    public static class ExceptionExtensions
    {
        private const string ERROR = "An unexpected error has occurred.";

        public static ErrorResult HandleError(this Exception ex)
        {
            return new ErrorResult(
                ERROR,
                new List<Error>
                {
                    new(ex.Message, ex.StackTrace ?? string.Empty)
                });
        }

        public static ErrorResult<T> HandleError<T>(this Exception ex)
        {
            return new ErrorResult<T>(
                ERROR,
                new List<Error>
                {
                    new(ex.Message, ex.StackTrace ?? string.Empty)
                });
        }
    }
}
