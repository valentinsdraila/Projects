using PmsSettings;
using System;
using System.Text;

namespace ScheduleCmd
{
    public sealed class MessageSender
    {
        private static ISettingsDao settings = new SettingsDao(); // this 'holds' the latest values/names for servers etc...

        public TheMessageToSend MessageInstance { get; set; }

        private Action<string> _logger;

        public MessageSender(Action<string> logger)
        {
            _logger = logger;
        }

        public void SendMessage()
        {
            try
            {
                var server = settings.GetFileServer();

                if (MessageInstance.SuccessfullyParsed && MessageInstance.SomeSanityCheck)
                {
                    using (var client = new System.Net.Sockets.TcpClient(server, 12345))
                    {
                        using (var stream = client.GetStream())
                        {
                            var buffer = Encoding.Unicode.GetBytes(MessageInstance.ToString());
                            stream.Write(buffer, 0, buffer.Length);
                        }
                    }
                    _logger($"Sucessfully sent message");
                }
                else
                {
                    _logger($"Unable to send, parsing was unsuccessful");
                }
            }
            catch (System.Net.Sockets.SocketException se)
            {
                _logger($"Some sort of connection issue: {se}");
                throw;
            }
            catch (Exception ex)
            {
                _logger($"Failed to send message: {ex}");
                throw;
            }
        }

    }
}
