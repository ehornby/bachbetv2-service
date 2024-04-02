using BachBetV2.Application.Models.Results;
using BachBetV2.Domain.Entities;

namespace BachBetV2.Application.Interfaces
{
    public interface ITagService
    {
        Task<Result> AddTagAsync(Tag tag, CancellationToken cancellationToken);
        Task<Result<List<Tag>?>> GetAllTagsAsync(CancellationToken cancellationToken);
    }
}