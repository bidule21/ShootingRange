using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ServiceLocation;
using ShootingRange.BusinessObjects;
using ShootingRange.Repository.RepositoryInterfaces;

namespace ShootingRange.Ranking
{
    public class RankingReportBuilder
    {
        private readonly RankingConfiguration _rankingConfiguration;

        public RankingReportBuilder(RankingConfiguration rankingConfiguration)
        {
            _rankingConfiguration = rankingConfiguration;
        }

        public RankingReport Build()
        {
            switch (_rankingConfiguration.RankingType)
            {
                case RankingType.SingleWithoutSeries:
                    return BuildSingleRankingWithoutSeriesReport();
                case RankingType.SingleWithSeries:
                    return BuildSingleRankingWithSeriesReport();
                    case RankingType.Collection:
                    return BuildCollectionRankingReport();
                default:
                    throw new InvalidOperationException(string.Format("Unknown Ranking type '{0}'.",
                        _rankingConfiguration.RankingType));
            }
        }

        private IEnumerable<Tuple<Person, int, decimal, decimal>> GetBestResults(int programNumber)
        {
            IShooterDataStore shooterDataStore = ServiceLocator.Current.GetInstance<IShooterDataStore>();
            IShooterParticipationDataStore shooterParticipationDataStore =
                ServiceLocator.Current.GetInstance<IShooterParticipationDataStore>();
            ISessionDataStore sessionDataStore = ServiceLocator.Current.GetInstance<ISessionDataStore>();
            ISessionSubtotalDataStore sessionSubtotalDataStore =
                ServiceLocator.Current.GetInstance<ISessionSubtotalDataStore>();
            IShotDataStore shotDataStore = ServiceLocator.Current.GetInstance<IShotDataStore>();
            IPersonDataStore personDataStore = ServiceLocator.Current.GetInstance<IPersonDataStore>();

            List<Shooter> shooters = (from s in shooterDataStore.GetAll()
                join sp in shooterParticipationDataStore.GetAll() on s.ShooterId equals sp.ShooterId
                where sp.ProgramNumber == programNumber
                select s).ToList();

            // Person, ShooterId, TotalScore, DeepShot
            List<Tuple<Person, int, decimal, decimal>> result = new List<Tuple<Person, int, decimal, decimal>>();

            foreach (Shooter shooter in shooters)
            {
                // Get the relevant sessions
                var lShooter = shooter;
                IEnumerable<Session> sessions = from s in sessionDataStore.GetAll()
                    where s.ShooterId == lShooter.ShooterId && s.ProgramNumber == programNumber
                    select s;

                // Get the results to the sessions
                var maxScoreGrouping = from session in sessions
                    join subTotal in sessionSubtotalDataStore.GetAll() on session.SessionId equals subTotal.SessionId
                    join shot in shotDataStore.GetAll() on subTotal.SessionSubtotalId equals shot.SubtotalId
                    group shot by session
                    into grouping
                    select new
                    {
                        Session = grouping.Key,
                        TotalScore = grouping.Sum(x => x.PrimaryScore)
                    };

                // Get the max scores of each session
                var bestGrouping = maxScoreGrouping.OrderByDescending(x => x.TotalScore).FirstOrDefault();

                if (bestGrouping != null)
                {
                    // select the max score as the result which shall go into ranking
                    decimal maxScore = bestGrouping.TotalScore;
                    decimal? deepShot = (from subTotal in sessionSubtotalDataStore.GetAll()
                        join shot in shotDataStore.GetAll() on subTotal.SessionSubtotalId equals shot.SubtotalId
                        where subTotal.SessionId == bestGrouping.Session.SessionId
                        orderby shot.SecondaryScore descending
                        select shot.SecondaryScore).FirstOrDefault();

                    if (lShooter.PersonId.HasValue)
                    {
                        Person person = personDataStore.FindById(lShooter.PersonId.Value);
                        result.Add(new Tuple<Person, int, decimal, decimal>(person, shooter.ShooterId, maxScore, deepShot ?? 0));
                    }
                }
            }

            return result;
        }

        private RankingReport BuildSingleRankingWithSeriesReport()
        {
            SingleRankingWithSeriesReport report = new SingleRankingWithSeriesReport();
            int progamNumber = _rankingConfiguration.ProgramNumber;
            IEnumerable<Tuple<Person, int, decimal, decimal>> bestResults = GetBestResults(progamNumber);

            foreach (Tuple<Person, int, decimal, decimal> result in bestResults)
            {
                report.Add(result.Item1, result.Item3, result.Item4);
            }

            return report;
        }

        private RankingReport BuildSingleRankingWithoutSeriesReport()
        {
            SingleRankingWithoutSeriesReport report = new SingleRankingWithoutSeriesReport();
            int progamNumber = _rankingConfiguration.ProgramNumber;
            IEnumerable<Tuple<Person, int, decimal, decimal>> bestResults = GetBestResults(progamNumber);

            foreach (Tuple<Person, int, decimal, decimal> result in bestResults)
            {
                report.Add(result.Item1, result.Item3);
            }

            return report;
        }

        private RankingReport BuildCollectionRankingReport()
        {
            IShooterCollectionDataStore shooterCollectionDataStore =
                ServiceLocator.Current.GetInstance<IShooterCollectionDataStore>();
                        ICollectionShooterDataStore collectionShooterDataStore =
                ServiceLocator.Current.GetInstance<ICollectionShooterDataStore>();

            CollectionRankingReport report = new CollectionRankingReport();
            int progamNumber = _rankingConfiguration.ProgramNumber;
            IEnumerable<Tuple<Person, int, decimal, decimal>> bestResults = GetBestResults(progamNumber);

            foreach (Tuple<Person, int, decimal, decimal> result in bestResults)
            {
                int shooterId = result.Item2;
                ShooterCollection shooterCollection = (from sc in shooterCollectionDataStore.GetAll()
                    join cs in collectionShooterDataStore.GetAll() on sc.ShooterCollectionId equals
                        cs.ShooterCollectionId
                    where cs.ShooterId == shooterId && sc.ProgramNumber == progamNumber
                    select sc).FirstOrDefault();

                if (shooterCollection != null)
                    report.Add(result.Item1, result.Item3, shooterCollection);
            }

            return report;
        }
    }
}