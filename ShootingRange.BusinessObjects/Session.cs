using System.Collections.Generic;
using System.Linq;

namespace ShootingRange.BusinessObjects
{
  public class Session
  {
    public int SessionId { get; set; }
    public int ShooterId { get; set; }
    public int LaneNumber { get; set; }
    public int ProgramNumber { get; set; }

    private List<SubSession> _subSessions;

    public Session()
    {
      _subSessions = new List<SubSession>();
    }

    public SubSession CreateSubSession()
    {
      SubSession subSession = new SubSession();
      subSession.SessionId = SessionId;
      subSession.Ordinal = _subSessions.Count;
      _subSessions.Add(subSession);
      return subSession;
    }

    public SubSession CurrentSubsession()
    {
      return _subSessions.LastOrDefault();
    }
  }

  public class SubSession
  {
    public int SessionSubtotalId { get; set; }
    public int Ordinal { get; set; }
    public int SessionId { get; set; }
    public int? BestShotId { get; set; }
  }
}
