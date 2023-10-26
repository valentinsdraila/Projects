using System;
using System.Collections.Generic;
using SherlockUtil;

namespace Sherlock
{
  class ConsoleMessage : IMessage
  {
    private class ConsoleFontColor : IDisposable
    {
      private static Dictionary<MessageColor, ConsoleColor> 
        colorMapping = new Dictionary<MessageColor, ConsoleColor> { { MessageColor.eGreen, ConsoleColor.Green },
                                                                    { MessageColor.eRed, ConsoleColor.Red } };

      private ConsoleColor oldColor;
      public ConsoleFontColor(MessageColor color)
      {
        this.oldColor = Console.ForegroundColor;
        Console.ForegroundColor = colorMapping[color];
      }
      public void Dispose()
      {
        Console.ForegroundColor = this.oldColor;
      }
    }
    public void Write(string text, MessageColor color, bool status=true)
    {
      var consoleColor = new ConsoleFontColor(color);
      Console.WriteLine(text);
      consoleColor.Dispose();
    }

    public void Write(string text, bool status = true)
    {
      Console.WriteLine(text);
    }
  }

}
