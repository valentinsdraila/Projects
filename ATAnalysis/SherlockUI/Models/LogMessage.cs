using System.Windows.Media;

namespace SherlockUI.Models
{
  public class LogMessage
  {
    public string Text { get; set; }
    public ImageSource ImgSource { get; set; }
    public LogMessage(string text, ImageSource imgSource)
    {
      Text = text;
      ImgSource = imgSource;
    }
    public LogMessage()
    {

    }
  }
}
