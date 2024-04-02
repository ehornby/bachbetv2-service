using BachBetV2.Application.Models.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BachBetV2.Application.Extensions
{
    public static class ResultExtensions
    {
        public static ContentResult GenerateErrorResponse(this ErrorResult errorResult)
        {
            ContentResult result = new()
            {
                Content = errorResult.Message,
                StatusCode = StatusCodes.Status500InternalServerError,
                ContentType = "text/plain"
            };

            return result;
        }

        public static ContentResult GenerateErrorResponse<T>(this ErrorResult<T> errorResult)
        {
            ContentResult result = new()
            {
                Content = errorResult.Message,
                StatusCode = StatusCodes.Status500InternalServerError,
                ContentType = "text/plain"
            };

            return result;
        }
    }
}
