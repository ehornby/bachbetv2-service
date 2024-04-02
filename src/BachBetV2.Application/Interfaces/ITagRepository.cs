using BachBetV2.Application.DTOs;

namespace BachBetV2.Application.Interfaces
{
    public interface ITagRepository
    {
        Task AddTag(TagDto dto, CancellationToken cancellationToken);
        Task<List<TagDto>?> GetAllTags(CancellationToken cancellationToken);
    }
}