using BachBetV2.Application.DTOs;
using BachBetV2.Application.Extensions;
using BachBetV2.Application.Interfaces;
using BachBetV2.Application.Models.Results;
using BachBetV2.Domain.Entities;

namespace BachBetV2.Application.Services
{
    public sealed class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;
        public TagService(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<Result<List<Tag>?>> GetAllTagsAsync(CancellationToken cancellationToken)
        {
            try
            {
                var dto = await _tagRepository.GetAllTags(cancellationToken);

                var tags = dto?.Select(t => new Tag()
                {
                    Id = t.TagId!,
                    Description = t.Description
                }).ToList();

                return new SuccessResult<List<Tag>?>(tags);
            }
            catch (Exception ex)
            {
                return ex.HandleError<List<Tag>?>();
            }
        }

        public async Task<Result> AddTagAsync(Tag tag, CancellationToken cancellationToken)
        {
            try
            {
                TagDto dto = new()
                {
                    TagId = tag.Id,
                    Description = tag.Description,
                };

                await _tagRepository.AddTag(dto, cancellationToken);

                return new SuccessResult();
            }
            catch (Exception ex)
            {
                return ex.HandleError();
            }
        }
    }
}
