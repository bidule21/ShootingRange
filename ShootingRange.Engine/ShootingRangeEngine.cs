using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using ShootingRange.BusinessObjects;
using ShootingRange.Common;
using ShootingRange.Repository.RepositoryInterfaces;

namespace ShootingRange.Engine
{
    public class ShootingRangeEngine
    {
        public event EventHandler<LogEventArgs> Log;

        private IShotDataStore _shotDataStore;
        private ISessionDataStore _sessionDataStore;
        private ISessionSubtotalDataStore _sessionSubtotalDataStore;
        private IShooterDataStore _shooterDataStore;
        private IPersonDataStore _personDataStore;
        private IShootingRange _shootingRange;

        private Dictionary<int, Session> _sessionsAwaitingProgramNumber;
        private Dictionary<int, Session> _sessionsOngoing;

        public ShootingRangeEngine(IContainer container)
        {
            _sessionsAwaitingProgramNumber = new Dictionary<int, Session>();
            _sessionsOngoing = new Dictionary<int, Session>();

            _shootingRange = container.Resolve<IShootingRange>();
            _shootingRange.Log += ShootingRangeOnLog;

            _sessionDataStore = container.Resolve<ISessionDataStore>();
            _sessionSubtotalDataStore = container.Resolve<ISessionSubtotalDataStore>();
            _shotDataStore = container.Resolve<IShotDataStore>();
            _shooterDataStore = container.Resolve<IShooterDataStore>();
            _personDataStore = container.Resolve<IPersonDataStore>();
        }

        private void ShootingRangeOnLog(object sender, LogEventArgs e)
        {
            OnLog(e);
        }

        private void LogMessage(string message)
        {
            OnLog(new LogEventArgs(message));
        }

        protected virtual void OnLog(LogEventArgs e)
        {
            EventHandler<LogEventArgs> handler = Log;
            if (handler != null) handler(this, e);

        }

        public bool IsProcessing { get; private set; }

        public void ConnectToSius()
        {
            _shootingRange.Initialize();
        }

        public void StartProcessing()
        {
            IsProcessing = true;
            _shootingRange.Prch += ShootingRangeOnPrch;
            _shootingRange.Shot += ShootingRangeOnShot;
            _shootingRange.BestShot += ShootingRangeOnBestShot;
            _shootingRange.Subt += ShootingRangeOnSubt;
        }

        private void ShootingRangeOnSubt(object sender, SubtEventArgs e)
        {
            if (_sessionsOngoing.ContainsKey(e.LaneNumber))
            {
                Session session = _sessionsOngoing[e.LaneNumber];
                session.CreateSubSession();
            }
        }

        private void ShootingRangeOnBestShot(object sender, ShotEventArgs e)
        {
            if (_sessionsOngoing.ContainsKey(e.LaneNumber))
            {
                SubSession currentSubSession = _sessionsOngoing[e.LaneNumber].CurrentSubsession();
                Shot shot =
                    _shotDataStore.FindBySubSessionId(currentSubSession.SessionSubtotalId)
                        .Single(_ => _.Ordinal == e.Ordinal);
                currentSubSession.BestShotId = shot.ShotId;
                _sessionSubtotalDataStore.Update(currentSubSession);
            }
        }

        /// <summary>Stores the shot data to the repository and invokes module extension points.</summary>
        private void ShootingRangeOnShot(object sender, ShotEventArgs e)
        {
            if (_sessionsAwaitingProgramNumber.ContainsKey(e.LaneNumber))
            {
                Session session = _sessionsAwaitingProgramNumber[e.LaneNumber];
                _sessionsAwaitingProgramNumber.Remove(e.LaneNumber);
                _sessionsOngoing.Add(e.LaneNumber, session);
                session.ProgramNumber = e.ProgramNumber;
                _sessionDataStore.Create(session);

                SubSession subSession = session.CreateSubSession();
                _sessionSubtotalDataStore.Create(subSession);

                AddShotToSubsession(e, subSession);
            }
            else if (_sessionsOngoing.ContainsKey(e.LaneNumber))
            {
                Session session = _sessionsOngoing[e.LaneNumber];
                SubSession subSession = session.CurrentSubsession();
                if (subSession.SessionSubtotalId == 0)
                    _sessionSubtotalDataStore.Create(subSession);
                AddShotToSubsession(e, subSession);
            }
            else
            {
                throw new InvalidOperationException("Session is not available.");
            }
        }

        private void AddShotToSubsession(ShotEventArgs e, SubSession subSession)
        {
            Shot shot = new Shot
            {
                PrimaryScore = e.PrimaryScore,
                SecondaryScore = e.SecondaryScore,
                LaneNumber = e.LaneNumber,
                SubtotalId = subSession.SessionSubtotalId,
                Ordinal = e.Ordinal,
            };
            _shotDataStore.Create(shot);
        }

        private void ShootingRangeOnPrch(object sender, PrchEventArgs e)
        {
            Shooter shooter = _shooterDataStore.FindByShooterNumber(e.ShooterNumber);
            if (shooter == null)
            {
                LogMessage(string.Format("ShooterNumber {0} not available. Creating shooter...", e.ShooterNumber));
                shooter = CreateUnknownShooter(e.ShooterNumber);
            }

            if (_sessionsOngoing.ContainsKey(e.LaneNumber))
            {
                _sessionsOngoing.Remove(e.LaneNumber);
            }

            _sessionsAwaitingProgramNumber.Add(e.LaneNumber,
                new Session
                {
                    LaneNumber = e.LaneNumber,
                    ShooterId = shooter.ShooterId,
                });
        }

        private Shooter CreateUnknownShooter(int shooterNumber)
        {
            Person person = new Person()
            {
                FirstName = "unknown",
                LastName = "unknown"
            };
            _personDataStore.Create(person);
            Shooter shooter = new Shooter();
            shooter.PersonId = person.PersonId;
            shooter.ShooterNumber = shooterNumber;
            _shooterDataStore.Create(shooter);
            return shooter;
        }
    }
}