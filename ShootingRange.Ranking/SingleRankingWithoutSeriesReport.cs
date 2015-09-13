using System;
using System.Collections.Generic;
using System.Linq;
using ShootingRange.BusinessObjects;

namespace ShootingRange.Ranking
{
    public class SingleRankingWithoutSeriesReport : RankingReport
    {
        private readonly List<Tuple<Person, decimal>> _rankItems;

        public SingleRankingWithoutSeriesReport()
        {
            _rankItems = new List<Tuple<Person, decimal>>();
        }

        public void Add(Person person, decimal score)
        {
            _rankItems.Add(new Tuple<Person, decimal>(person, score));
        }

        public override ICollection<RankItem> GetOrderedRankItems()
        {
            return _rankItems.OrderByDescending(x => x.Item2).Select((x, index) => new RankItem
            {
                Label = string.Format("{0} {1}", x.Item1.FirstName, x.Item1.LastName),
                Score = x.Item2,
                Rank = index+1
            }).ToList();
        } 
    }

    public abstract class RankingReport
    {
        public abstract ICollection<RankItem> GetOrderedRankItems();
    }

    public class CollectionRankingReport : RankingReport
    {
        private readonly Dictionary<int, string> _shooterCollectionNames = new Dictionary<int, string>();

        private readonly List<Tuple<Person, decimal, int>> _rankItems;

        public CollectionRankingReport()
        {
            _rankItems = new List<Tuple<Person, decimal, int>>();
            _shooterCollectionNames = new Dictionary<int, string>();
        }


        public void Add(Person person, decimal score, ShooterCollection shooterCollection)
        {
            if (!_shooterCollectionNames.ContainsKey(shooterCollection.ShooterCollectionId))
                _shooterCollectionNames.Add(shooterCollection.ShooterCollectionId, shooterCollection.CollectionName);
            _rankItems.Add(new Tuple<Person, decimal, int>(person, score, shooterCollection.ShooterCollectionId));
        }

        public override ICollection<RankItem> GetOrderedRankItems()
        {
            var grouped = from sc in _shooterCollectionNames join ri in _rankItems on sc.Key equals ri.Item3 group ri by sc;

            var ordered = grouped.OrderByDescending(x => x.Sum(y => y.Item2)).ThenByDescending(x => x.Max(y => y.Item2));

            List<RankItem> rankItems = new List<RankItem>();
            int rank = 1;

            foreach (IGrouping<KeyValuePair<int, string>, Tuple<Person, decimal, int>> grouping in ordered)
            {
                string colletionShooters = string.Join("\r\n\t", grouping.Select(x => string.Format("{0} {1} ({2})", x.Item1.FirstName, x.Item1.LastName, (int)x.Item2)).ToArray());
                rankItems.Add(new RankItem
                {
                    Label = string.Format("{0}\r\n\t{1}", grouping.Key.Value, colletionShooters),
                    Rank = rank++,
                    Score = (int)grouping.Sum(x => x.Item2)
                });
            }

            return rankItems;
        }
    }

    public class SingleRankingWithSeriesReport : RankingReport
    {
        private readonly List<Tuple<Person, decimal, decimal>> _rankItems;

        public SingleRankingWithSeriesReport()
        {
            _rankItems = new List<Tuple<Person, decimal, decimal>>();
        }


        public void Add(Person person, decimal score, decimal deepShot)
        {
            _rankItems.Add(new Tuple<Person, decimal, decimal>(person, score, deepShot));
        }

        public override ICollection<RankItem> GetOrderedRankItems()
        {
            return _rankItems.OrderByDescending(x => x.Item2).ThenByDescending(x => x.Item3).Select((x, index) => new RankItem
            {
                Label = string.Format("{0} {1}", x.Item1.FirstName, x.Item1.LastName),
                Score = x.Item2,
                Rank = index + 1,
                DeepShot = x.Item3.ToString()
            }).ToList();
        }
    }

    public class RankItem
    {
        public string Label { get; set; }

        public decimal Score { get; set; }

        public int Rank { get; set; }

        public string DeepShot { get; set; }
    }
}