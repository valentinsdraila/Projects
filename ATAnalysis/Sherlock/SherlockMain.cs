using System;
using System.Xml;
using SherlockUtil;

namespace Sherlock
{
  class SherlockMain
  {
    public static string iniPath = @"..\..\..\Config\Sherlock.ini";
    static void Main(string[] args)
    {
      int consoleKey = -1;
      ReadWriteINIfile iniUtil = new ReadWriteINIfile(iniPath);
      while (consoleKey != 0)
      {
        Console.WriteLine("Choose the action you want to perform:");
        Console.WriteLine("1. Create the AT Report");
        Console.WriteLine("2. Reschedule tests");

        consoleKey = Convert.ToInt32(Console.ReadLine());
        switch (consoleKey)
        {
          case 1:
          {
            var message = new ConsoleMessage();
            Console.WriteLine("Starting analysis.");
            XmlDocument failedTestsXML = FailedTests.Retrieve(iniUtil, message, args);
            if (failedTestsXML.HasChildNodes == false || failedTestsXML == null)
            {
              message.Write("Error while retrieving failed tests data", MessageColor.eRed);
              return;
            }
            ATReport atReport = new ATReport(failedTestsXML);
            if (atReport.Create(iniPath, message))
            {
              atReport.CompareToLastReport(message);
              atReport.SaveReport(message);
            }
            GC.Collect();
            break;
          }

          case 2:
          {
            var message = new ConsoleMessage();
            string watsonOutputPath = iniUtil.ReadINI("GlobalSettings", "WatsonOutputPath")[0];
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(watsonOutputPath);
            ATReport rescheduleATReport = new ATReport(xmlDocument);
            rescheduleATReport.RescheduleTests(message);
            GC.Collect();
            break;
          }
        }

      }
    }
  }
}
