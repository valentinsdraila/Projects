using System;

namespace ScheduleCmd
{
    public class Program
    {
        static int Main(string[] args)
        {
            /*
             * For now only Debug supported
             * File @"C:\ProgramData\pms\settings.ini" is needed
             *  * available on dev systems (if sidekick or ATF is installed)
             * TODO: use args to fill the properties
             * 
             */

            try
            {
                // Input for message
                // This example uses a couple of defaults. Do not deviate if not needed.
                // e.g. don't enable picasso coverage as this is no longer supported.
                var scenarioID = int.Parse(args[0]);
                var build = args[1];
                var testSystem = args[2]; // empty (=Any) or a test system name
                var user = Environment.UserName; // this user, or pass another user...

                // Generation of message
                var mess = new TheMessageToSend(Log);
                mess.SetLabelProps(build);
                mess.ScenarioId = scenarioID;
                mess.TestSystemName = testSystem;
                mess.User = user;


                // Sending
                var sender = new MessageSender(Log);
                sender.MessageInstance = mess;

                sender.SendMessage();
            }
            catch (System.Net.Sockets.SocketException se)
            {
                Log($"Some sort of connection issue: {se}");
                throw;
            }
            catch (Exception e)
            {
                // can be finetuned
                Log(e.ToString());
                return 1;
            }
            return 0;
        }

        private static void Log(string message)
        {
            var timeStamp = $"{DateTime.Now:F}";
            Console.WriteLine($"{timeStamp}: {message}");
        }
        
    }
}
