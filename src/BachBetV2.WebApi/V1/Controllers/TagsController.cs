using BachBetV2.Application.Extensions;
using BachBetV2.Application.Interfaces;
using BachBetV2.Application.Models.Results;
using BachBetV2.Domain.Entities;
using BachBetV2.Domain.Models.Requests;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BachBetV2.WebApi.V1.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet]
        [Route((""))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllTags(CancellationToken cancellationToken = default)
        {
            return await _tagService.GetAllTagsAsync(cancellationToken) switch
            {
                SuccessResult<List<Tag>> successResult => new OkObjectResult(successResult.Data),
                ErrorResult<List<Tag>> errorResult => errorResult.GenerateErrorResponse(),
                _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
            };
        }

        [HttpPost]
        [Route((""))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostTag(
            [FromBody, BindRequired] CreateTagRequest request,
            [FromServices] IValidator<CreateTagRequest> validator,
            CancellationToken cancellationToken = default)
        {
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                return new BadRequestObjectResult(validationResult.Errors);
            }

            Tag tag = new()
            {
                Description = request.Description
            };

            return await _tagService.AddTagAsync(tag, cancellationToken) switch
            {
                SuccessResult => new OkResult(),
                ErrorResult errorResult => errorResult.GenerateErrorResponse(),
                _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
            };
        }
    }
}
