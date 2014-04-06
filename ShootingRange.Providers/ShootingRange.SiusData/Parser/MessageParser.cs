using System;
using System.Collections.Generic;
using System.Linq;
using ShootingRange.SiusData.Messages;

namespace ShootingRange.SiusData.Parser
{
  public class MessageParser
  {
    private readonly List<IMessageParser> _messageParser;

    /// <summary>Message parser for sius data messages. </summary>
    public MessageParser(MessageFactory businessObjectFactory)
    {
      _messageParser = new List<IMessageParser>();
      _messageParser.Add(new PrchMessageParser(businessObjectFactory));
      _messageParser.Add(new ShotMessageParser(businessObjectFactory));
      _messageParser.Add(new TotalMessageParser(businessObjectFactory));
      _messageParser.Add(new SubtotalMessageParser(businessObjectFactory));
    }

    public List<SiusDataMessage> Parse(string source)
    {
      List<SiusDataMessage> messages = new List<SiusDataMessage>();

      string[] stringList = source.Split(new[] { "\r\n" }, StringSplitOptions.None);
      if (stringList.Length > 0)
      {
        foreach (string s in stringList)
        {
          string[] values = s.Split(new[] { ';' }, StringSplitOptions.None);
          if (values.Length > 1)
          {
            string key = values[0];
            List<string> valueList = Enumerable.Range(1, values.Length - 1).Select(_ => values[_]).ToList();

            RawMessage rawMessage = new RawMessage {Identifier = key, Values = valueList};

            foreach (IMessageParser parser in _messageParser)
            {
              SiusDataMessage message = parser.Parse(rawMessage);
              if (message != null)
                messages.Add(message);
            }
          }
        }
      }

      return messages;
    }
  }
}
