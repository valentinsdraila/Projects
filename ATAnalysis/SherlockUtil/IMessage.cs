namespace SherlockUtil
{
  public enum MessageColor
  {
    eUnchanged,
    eGreen,
    eRed
  };
  public interface IMessage
  {
    void Write(string text, bool status);
    void Write(string text, MessageColor color, bool status);
  }
}
