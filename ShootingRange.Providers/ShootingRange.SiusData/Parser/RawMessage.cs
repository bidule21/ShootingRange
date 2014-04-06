using System.Collections.Generic;

namespace ShootingRange.SiusData.Parser
{
  class RawMessage
  {
    public string Identifier { get; set; }
    public IEnumerable<string> Values { get; set; }
  }
}
