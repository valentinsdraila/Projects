using System.Xml;

namespace SherlockUtil
{
  public class FailedTests
  {
    public static XmlDocument Retrieve(ReadWriteINIfile iniUtil, IMessage message, string[] args)
    {
      XmlDocument failedTestsXML = new XmlDocument();
      if (args != null)
      {
        if (args.Length > 0)
        {
          if (args[0] == "-W" || args[0] == "-w")
            try
            {
              failedTestsXML.Load(args[1]);
            }
            catch (System.Xml.XmlException)
            {
              message.Write("Invalid input file", false);
            }
          if (args[0] == "-n" || args[0] == "-N")
          {
            iniUtil.CreateINIFile();
            string exePath = "Watson.exe";
            string watsonOutputPath = iniUtil.ReadINI("GlobalSettings", "WatsonOutputPath")[0];
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.FileName = exePath;
            process.StartInfo.Arguments = watsonOutputPath;
            process.StartInfo.Arguments += " " + iniUtil.path;
            process.Start();

            message.Write("Retrieving failed test data...", true);

            process.WaitForExit();

            if (process.ExitCode == 0)
            {
              message.Write("Data collected successfully!", MessageColor.eGreen, true);
              message.Write("Creating the report...", true);
            }
            else
            {
              message.Write("There were errors while collecting data!", MessageColor.eRed, false);
            }
            failedTestsXML.Load(watsonOutputPath);
          }
        }
        else
        {

          string exePath = "Watson.exe";
          string watsonOutputPath = iniUtil.ReadINI("GlobalSettings", "WatsonOutputPath")[0];
          System.Diagnostics.Process process = new System.Diagnostics.Process();
          process.StartInfo.UseShellExecute = false;
          process.StartInfo.FileName = exePath;
          process.StartInfo.Arguments = watsonOutputPath;
          process.Start();

          message.Write("Retrieving failed test data...", true);

          process.WaitForExit();

          if (process.ExitCode == 0)
          {
            message.Write("Data collected successfully!", MessageColor.eGreen, true);
            message.Write("Creating the report...", true);
          }
          else
          {
            message.Write("There were errors while collecting data!", MessageColor.eRed, false);
          }

          failedTestsXML.Load(watsonOutputPath);
        }
      }
      return failedTestsXML;
    }
  }
}
