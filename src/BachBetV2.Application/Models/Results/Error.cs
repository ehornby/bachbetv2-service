namespace BachBetV2.Application.Models.Results
{
    public sealed class Error
    {
        public string? Code { get; }
        public string Details { get; }

        public Error(string details) : this(null, details)
        {
        }

        public Error(string? code, string details)
        {
            Code = code;
            Details = details;
        }
    }
}
