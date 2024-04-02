
namespace BachBetV2.Application.Configuration
{
    public sealed record DynamoDbOptions
    {
        public string? UserTableName { get; init; }
        public string? BetTableName { get; init; }

        public string? AwsAccessKeyId { get; init; }
        public string? AwsSecretAccessKey { get; init; }
    }
}
