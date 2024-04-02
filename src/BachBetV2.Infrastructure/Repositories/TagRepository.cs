using BachBetV2.Application.DTOs;
using BachBetV2.Application.Interfaces;
using BachBetV2.Infrastructure.Database.Contexts;
using BachBetV2.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace BachBetV2.Infrastructure.Repositories
{
    public sealed class TagRepository : ITagRepository
    {
        private readonly BachBetContext _context;
        public TagRepository(BachBetContext context)
        {
            _context = context;
        }

        public async Task<List<TagDto>?> GetAllTags(CancellationToken cancellationToken)
        {
            List<TagDto> dto = await _context.Tags.Select(t => new TagDto()
            {
                TagId = t.Id.ToString(),
                Description = t.TagDescription
            }).ToListAsync(cancellationToken);

            if (dto is not null && dto.Count > 0)
            {
                return dto;
            }

            return null;
        }

        public async Task AddTag(TagDto dto, CancellationToken cancellationToken)
        {
            TagEntity entity = new()
            {
                TagDescription = dto.Description!
            };

            _context.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
