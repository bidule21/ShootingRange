using System.Linq;

namespace ShootingRange.SiusData.Parser
{
  static class ParseHelper
  {
    public static bool IsResponsible(this RawMessage rawMessage, string identifier, int valueCount)
    {
      return rawMessage.Identifier == identifier && (rawMessage.Values.Count() == valueCount || valueCount == -1);
    }
  }
}
