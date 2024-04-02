namespace BachBetV2.Domain.Enums
{
    public enum BetResult
    {
        Happened,
        DidNotHappen
    }

    public static class BetResultExtensions
    {
        public static bool Happened(this BetResult result)
        {
            return result == BetResult.Happened;
        }
    }
}
