using SherlockUtil;
using System;
using static SherlockUI.ViewModels.MainWindowViewModel;

namespace SherlockUI.Models
{
  class WindowMessage : IMessage
  {
    private TextBoxCallback textboxContentCallback;

    public WindowMessage(TextBoxCallback textBoxCallback)
    {
      textboxContentCallback = textBoxCallback;
    }
    public void Write(string text, bool status)
    {
      text = DateTime.Now.ToString("HH:mm:ss") + " " + text;
      textboxContentCallback?.Invoke(text, status);
    }

    public void Write(string text, MessageColor color, bool status)
    {
      text = DateTime.Now.ToString("HH:mm:ss") + " "+ text;
      textboxContentCallback?.Invoke(text, status);
    }
  }
}
