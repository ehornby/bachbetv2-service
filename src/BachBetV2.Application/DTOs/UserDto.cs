using System.Text.Json.Serialization;

namespace BachBetV2.Application.DTOs
{
    public sealed class UserDto
    {
        public string UserId { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public Guid Token { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Password { get; set; }
    }
}
