using System.Runtime.InteropServices;

namespace ShootingRange.Ranking
{
    public class RankingConfigurationBuilder
    {
        private readonly int _programNumber;
        private readonly RankingType _rankingType;

        public RankingConfigurationBuilder(int programNumber, RankingType rankingType)
        {
            _programNumber = programNumber;
            _rankingType = rankingType;
        }

        public static implicit operator RankingConfiguration(RankingConfigurationBuilder builder)
        {
            return new RankingConfiguration
            {
                ProgramNumber = builder._programNumber,
                RankingType = builder._rankingType,
            };
        }
    }
}