using ShootingRange.SiusData.Messages;

namespace ShootingRange.SiusData.Parser
{
  interface IMessageParser
  {
    SiusDataMessage Parse(RawMessage rawMessage);
  }
}
