namespace ShootingRange.Ranking
{
    public class RankingConfiguration
    {
        public RankingType RankingType { get; set; }
        public int ProgramNumber { get; set; }
    }

    public enum RankingType
    {
        None,
        SingleWithSeries,
        SingleWithoutSeries,
        Collection
    }
}