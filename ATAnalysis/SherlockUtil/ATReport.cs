using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Xml;
using Excel = Microsoft.Office.Interop.Excel;

namespace SherlockUtil
{
  public class ATReport
  {
    public enum columnType
    {
      eNone,
      eCategory,
      eTest,
      eErrorMessage,
      eInvestigator,
      eStatus,
      eComments,
      eResponsible,
      eInput,
      eHostName,
      eIMG,
      eImageText,
      eMOV
    };

    public struct ReportColumn
    {
      public columnType ID { get; set; }
      public string Label { get; set; }
      public int Width { get; set; }

      public ReportColumn(columnType id, string label, int width)
      {
        ID = id;
        Label = label;
        Width = width;
      }
    }

    private static List<ReportColumn> reportColumns = new List<ReportColumn>(){
                { new ReportColumn(columnType.eCategory, "Category", 50)},
                { new ReportColumn(columnType.eTest, "Test", 50)},
                { new ReportColumn(columnType.eErrorMessage, "ErrorMessage", 100)},
                { new ReportColumn(columnType.eInvestigator, "Investigator", 50)},
                { new ReportColumn(columnType.eStatus, "Status", 80)},
                { new ReportColumn(columnType.eComments, "Comments", 50)},
                { new ReportColumn(columnType.eResponsible, "Responsible", 50)},
                { new ReportColumn(columnType.eInput, "Input", 50)},
                { new ReportColumn(columnType.eHostName, "HostName", 50)},
                { new ReportColumn(columnType.eIMG, "IMG", 50)},
                { new ReportColumn(columnType.eImageText, "Image Text", 50)},
                { new ReportColumn(columnType.eMOV, "MOV", 50)}
            };
    public static List<ReportColumn> ReportColumns
    {
      get
      {
        return reportColumns;
      }
      set
      {
        reportColumns = value;
      }
    }
    private readonly string timeoutMessage = "The test object timed out.";
    private readonly List<string> statuses = new List<string>() { "Unresolved", "Assigned", "WaitingForFix", "To reschedule", "To update ref", "Rescheduled", "Ref updated", "Resolved" };
    private readonly List<string> hostNames = new List<string>() { "MACHINE01", "MACHINE02", "MACHINE03", "MACHINE04", "MACHINE05", "MACHINE06", "MACHINE07", "MACHINE08", "MACHINE09", "MACHINE10", "MACHINE11", "MACHINE12", "MACHINE13", "MACHINE14", "MACHINE15", "MACHINE16", "MACHINE17", "MACHINE18", "MACHINETST04", "MACHINETST05"};
    public XmlNode atReport { get; set; }
    public Excel.Application excelApp;
    public Excel.Workbook reportWorkbook;

    private Action<float> progressCallback;
    public Action<float> ProgressCallback
    {
      set
      {
        progressCallback = value;
      }
    }

    public ATReport(XmlDocument failedTestsXml)
    {
      atReport = failedTestsXml.SelectSingleNode("ATReport");
    }

    private void CreateSheet(Excel._Worksheet reportSheet, List<ReportColumn> reportColumns)
    {
      foreach (ReportColumn column in reportColumns)
      {
        reportSheet.Columns[column.ID].ColumnWidth = column.Width;
        reportSheet.Cells[2, column.ID] = column.Label;
      }
      Excel.Range labelsRange = reportSheet.Range["A2:L2"];
      labelsRange.Font.Size = 20;
      labelsRange.Font.Bold = true;
      labelsRange.Font.Color = System.Drawing.Color.Blue;
      labelsRange.HorizontalAlignment = 3;
    }
    public bool Create(string iniPath, IMessage message)
    {
      try
      {
        excelApp = new Excel.Application();
        reportWorkbook = excelApp.Workbooks.Add();
        ReadWriteINIfile iniUtil = new ReadWriteINIfile(iniPath);
        bool enableTextExtraction = int.Parse(iniUtil.ReadINI("GlobalSettings", "EnableTextExtraction")[0]) != 0;
        List<KeyValuePair<string, string>> outputFiles = new List<KeyValuePair<string, string>>();
        var ATOutputFiles = iniUtil.GetKeys("ATOutputFiles");
        foreach (var file in ATOutputFiles)
        {
          outputFiles.Add(new KeyValuePair<string, string>(file, iniUtil.ReadINI("ATOutputFiles", file)[0]));
        }
        float progress = 50;
        float total = int.Parse(atReport.SelectSingleNode("ScenarioCount").InnerText);
        float testObjectsTotal = int.Parse(atReport.SelectSingleNode("ObjectCount").InnerText);
        if (total == 0)
        {
          progress = 100;
          if (progressCallback != null)
          {
            progressCallback(progress);
          }
        }
        foreach (XmlNode atGroup in atReport.ChildNodes)
        {
          if (atGroup.Name != "ATGroup")
          { continue; }
          List<Task> tasks = new List<Task>();
          reportWorkbook.Sheets.Add();
          Excel._Worksheet reportSheet = reportWorkbook.ActiveSheet;
          reportSheet.Cells[1, 1] = atGroup.SelectSingleNode("LastBuildReported").InnerText;
          reportSheet.Name = atGroup.Attributes.GetNamedItem("Name").InnerText;

          CreateSheet(reportSheet, ReportColumns);

          int excelCellIndex = 3;
          object dictionaryLock = new object();
          SortedDictionary<string, List<TestObject>> testObjectErrorsDictionary = new SortedDictionary<string, List<TestObject>>();

          foreach (XmlNode testScenario in atGroup.ChildNodes)
          {
            if (testScenario.Name != "TestScenario")
            { continue; }
            foreach (XmlNode XmlTestObject in testScenario.ChildNodes)
            {
              if (XmlTestObject.Name != "TestObject")
                continue;
              TestObject testObject = new TestObject(XmlTestObject);
              testObject.HostName = testScenario.SelectSingleNode("HostName").InnerText;
              List<OutputFile> testOutputFiles = testObject.GetOutputFiles(outputFiles, iniPath, message);
              if (testObject.Status.Contains("timed out"))
              {
                if (testObjectErrorsDictionary.ContainsKey(timeoutMessage))
                {
                  testObjectErrorsDictionary[timeoutMessage].Add(testObject);
                }
                else
                  testObjectErrorsDictionary.Add(timeoutMessage, new List<TestObject>() { testObject });
                continue;
              }
              if (testOutputFiles.Count == 0)
              {
                reportSheet.Cells[excelCellIndex, columnType.eCategory] = testObject.Category;
                reportSheet.Cells[excelCellIndex, columnType.eTest] = testObject.Name;
                reportSheet.Cells[excelCellIndex, columnType.eInput] = testObject.Name;
                reportSheet.Cells[excelCellIndex, columnType.eErrorMessage] = "The output of the test does not exist anymore.";
                excelCellIndex++;
                testObjectsTotal--;
                continue;
              }
              tasks.Add(Task.Run(() =>
              {

                string errorMessage = testObject.ExtractError(testOutputFiles);
                lock (dictionaryLock)
                {
                  if (testObjectErrorsDictionary.ContainsKey(errorMessage))
                  {
                    testObjectErrorsDictionary[errorMessage].Add(testObject);
                  }
                  else
                  {
                    testObjectErrorsDictionary.Add(errorMessage, new List<TestObject>() { testObject });
                  }
                }
              }));
            }
            progress += 30 / total;
            if (progressCallback != null)
            {
              progressCallback(progress);
            }
          }
          Task.WaitAll(tasks.ToArray());
          foreach (var testObjectError in testObjectErrorsDictionary)
          {
            if (testObjectError.Value.Count > 1)
            {
              string mergeRangeAddress = string.Format("C{0}:C{1}", excelCellIndex, excelCellIndex + testObjectError.Value.Count - 1);
              Excel.Range mergeRange = reportSheet.Range[mergeRangeAddress];
              mergeRange.Merge();
              mergeRange.Value = testObjectError.Key;
              mergeRange.WrapText = true;
              for (int i = 0; i < testObjectError.Value.Count; i++)
              {
                try
                {
                  if (enableTextExtraction)
                  {
                    string imgError = testObjectError.Value[i].ExtractImage();
                    if (imgError != "")
                      reportSheet.Cells[excelCellIndex + i, columnType.eImageText] = imgError;
                  }
                }
                catch (Exception e)
                {
                  message.Write(e.Message, false);
                }
              }
            }
            else
            {
              reportSheet.Cells[excelCellIndex, columnType.eErrorMessage] = testObjectError.Key;

              try
              {
                if (enableTextExtraction)
                {
                  string imageError = testObjectError.Value[0].ExtractImage();
                  if (imageError != "")
                    reportSheet.Cells[excelCellIndex, columnType.eImageText] = imageError;
                }
              }
              catch (Exception e)
              {
                message.Write(e.Message, false);
              }
            }

            foreach (TestObject testObject in testObjectError.Value)
            {

              reportSheet.Cells[excelCellIndex, columnType.eCategory] = testObject.Category;
              reportSheet.Hyperlinks.Add(reportSheet.Cells[excelCellIndex, columnType.eTest], testObject.OutputFolder, Type.Missing,
                  "Go to TestObject Output", testObject.Name);
              reportSheet.Hyperlinks.Add(reportSheet.Cells[excelCellIndex, columnType.eInput], testObject.InputFolder, Type.Missing,
                  "Go to TestObject Input", testObject.Name);
              reportSheet.Cells[excelCellIndex, columnType.eResponsible] = testObject.Responsible;

              var formattedItems = string.Join(", ", statuses.ToArray());
              Excel.Range statusCellRange = (Microsoft.Office.Interop.Excel.Range)reportSheet.Cells[excelCellIndex, columnType.eStatus];
              statusCellRange.Validation.Add(Excel.XlDVType.xlValidateList, Excel.XlDVAlertStyle.xlValidAlertInformation, Excel.XlFormatConditionOperator.xlBetween, formattedItems, Type.Missing);
              statusCellRange.Value = "Unresolved";

              var formattedHostNames = string.Join(", ", hostNames.ToArray());
              Excel.Range hostNameCellRange = (Excel.Range)reportSheet.Cells[excelCellIndex, columnType.eHostName];
              hostNameCellRange.Validation.Add(Excel.XlDVType.xlValidateList, Excel.XlDVAlertStyle.xlValidAlertInformation, Excel.XlFormatConditionOperator.xlBetween, formattedHostNames, Type.Missing);
              hostNameCellRange.Value = testObject.HostName.ToUpper();

              string path = testObject.OutputFolder;
              string[] pathDirs = path.Split('\\');
              string[] newPathDirs = new string[pathDirs.Length - 1];
              Array.Copy(pathDirs, newPathDirs, pathDirs.Length - 1);
              string newPath = string.Join("\\", newPathDirs);
              newPath = newPath.Replace("Output", "Video");
              if (Directory.Exists(newPath))
              {
                string[] aviFiles = Directory.GetFiles(newPath, "*.avi");
                if (aviFiles.Length > 0)
                {
                  string videoPicPath = AppDomain.CurrentDomain.BaseDirectory + "\\Video.jpg";
                  const float videoIconSize = 25;
                  Excel.Range videoCellRange = (Microsoft.Office.Interop.Excel.Range)reportSheet.Cells[excelCellIndex, columnType.eMOV];
                  var videoIcon = reportSheet.Shapes.AddPicture(videoPicPath, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, (float)videoCellRange.Left + 2, (float)videoCellRange.Top + 2, videoIconSize, videoIconSize);
                  //videoCellRange.Rows[1].RowHeight = videoIcon.Height;
                  //videoCellRange.Columns[1].ColumnWidth = videoIcon.Width;
                  videoCellRange.Hyperlinks.Add(videoIcon, aviFiles[0], "", "Video for " + testObject.Name);
                }
              }

              string pathToATFSystem = testObject.OutputFolder + @"\ATFSystem";
              if (Directory.Exists(pathToATFSystem))
              {
                string[] jpgFiles = Directory.GetFiles(pathToATFSystem, "*.jpg");
                if (jpgFiles.Length > 0)
                {
                  string imgPicPath = AppDomain.CurrentDomain.BaseDirectory + "\\Img.jpg";
                  const float imgIconSize = 25;
                  Excel.Range imageCellRange = (Microsoft.Office.Interop.Excel.Range)reportSheet.Cells[excelCellIndex, columnType.eIMG];
                  var imageIcon = reportSheet.Shapes.AddPicture(imgPicPath, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, (float)imageCellRange.Left + 2, (float)imageCellRange.Top + 2, imgIconSize, imgIconSize);
                  imageCellRange.Hyperlinks.Add(imageIcon, jpgFiles[0], "", "Image for " + testObject.Name);

                }
              }
              
              excelCellIndex++;
              progress += 20 / testObjectsTotal;
              if (progressCallback != null)
              {
                progressCallback(progress);
              }
            }
          }
          reportSheet.Columns.AutoFit();
          reportSheet.Rows.AutoFit();
          reportSheet.Columns.VerticalAlignment = 2;
          Marshal.ReleaseComObject(reportSheet);
        }
        message.Write("Report created successfully!", MessageColor.eGreen, true);
        return true;
      }
      catch (Exception e)
      {
        message.Write(e.Message, false);
        message.Write("Error while creating the report", MessageColor.eRed, false);
        return false;
      }
    }
    public bool CompareToLastReport(IMessage message)
    {
      message.Write("Comparing to the last report...", true);

      string[] excelFiles = Directory.GetFiles(Directory.GetCurrentDirectory());
      if (excelFiles.Length < 1)
        return true;
      string excelFile = excelFiles.LastOrDefault(f => f.Contains("ATReport.xlsx") == true && f.Contains("$") == false);
      if (excelFile != null)
      {
        Excel.Application pastExcelApp = new Excel.Application();
        Excel.Workbook pastWorkbook = null;
        try
        {
          pastWorkbook = pastExcelApp.Workbooks.Open(excelFile);
        }
        catch(Exception e)
        {
          message.Write(e.Message, false);
          pastExcelApp.Quit();
          Marshal.ReleaseComObject(pastExcelApp);
          pastExcelApp = null;
          message.Write("Could not open the latest report", false);
          return false;
        }
        try
        {
          List<string> worksheets = new List<string>() { "" };
          if (pastWorkbook.Sheets.Count <= reportWorkbook.Sheets.Count)
            for (int i = 1; i < pastWorkbook.Sheets.Count; i++)
              worksheets.Add(pastWorkbook.Sheets[i].Name);
          else
          if (pastWorkbook.Sheets.Count > reportWorkbook.Sheets.Count)
            for (int i = 1; i < reportWorkbook.Sheets.Count; i++)
              worksheets.Add(reportWorkbook.Sheets[i].Name);

          for (int i = 1; i < worksheets.Count; i++)
          {
            var currentWorksheet = (Excel.Worksheet)reportWorkbook.Worksheets[worksheets[i]];
            var pastWorksheet = (Excel.Worksheet)pastWorkbook.Worksheets[worksheets[i]];
            Dictionary<string, Tuple<string, string, string>> testObjectInvestigatorComments = new Dictionary<string, Tuple<string, string, string>>();
            for (int row = 3; row <= pastWorksheet.UsedRange.Rows.Count; row++)
            {
              Tuple<string, string, string> InvestigatorComments = new Tuple<string, string, string>(pastWorksheet.Cells[row, columnType.eInvestigator].Value?.ToString(), pastWorksheet.Cells[row, columnType.eComments].Value?.ToString(), pastWorksheet.Cells[row, columnType.eStatus].Value?.ToString());
              if (!testObjectInvestigatorComments.ContainsKey(pastWorksheet.Cells[row, columnType.eTest].Value))
                testObjectInvestigatorComments.Add(pastWorksheet.Cells[row, columnType.eTest].Value, InvestigatorComments);
            }

            for (int row = 3; row <= currentWorksheet.UsedRange.Rows.Count; row++)
            {
              if (testObjectInvestigatorComments.ContainsKey(currentWorksheet.Cells[row, columnType.eTest].Value))
              {
                currentWorksheet.Cells[row, columnType.eInvestigator] = testObjectInvestigatorComments[currentWorksheet.Cells[row, columnType.eTest].Value].Item1;
                currentWorksheet.Cells[row, columnType.eComments] = testObjectInvestigatorComments[currentWorksheet.Cells[row, columnType.eTest].Value].Item2;
                if(pastWorksheet.Cells[1,1].Value == currentWorksheet.Cells[1,1].Value)
                currentWorksheet.Cells[row, columnType.eStatus] = testObjectInvestigatorComments[currentWorksheet.Cells[row, columnType.eTest].Value].Item3;
              }
            }
            Marshal.ReleaseComObject(pastWorksheet);
            Marshal.ReleaseComObject(currentWorksheet);
          }

          pastWorkbook.Close();
          pastExcelApp.Quit();
          Marshal.ReleaseComObject(pastExcelApp);
          Marshal.ReleaseComObject(pastWorkbook);
          pastWorkbook = null;
          pastExcelApp = null;

          message.Write("Reports compared successfully!", MessageColor.eGreen, true);
          message.Write("Saving the report...", true);
          return true;
        }
        catch (Exception e)
        {
          message.Write(e.Message, false);
          pastWorkbook.Close();
          pastExcelApp.Quit();
          Marshal.ReleaseComObject(pastExcelApp);
          Marshal.ReleaseComObject(pastWorkbook);
          pastWorkbook = null;
          pastExcelApp = null;

          message.Write("Error while comparing the reports", MessageColor.eRed, false);
          return false;
        }
      }
      else
        return true;
    }
    public void SaveReport(IMessage message, bool calledFromUI = false)
    {
      DateTime date = DateTime.Now;
      string dateString = date.ToString("yy_MM_dd");
      try
      {
        excelApp.DisplayAlerts = false;
        excelApp.ActiveWorkbook.SaveAs(AppDomain.CurrentDomain.BaseDirectory + "ATReport.xlsx", Excel.XlFileFormat.xlOpenXMLWorkbook, ConflictResolution: Excel.XlSaveConflictResolution.xlLocalSessionChanges);
        excelApp.ActiveWorkbook.SaveCopyAs(AppDomain.CurrentDomain.BaseDirectory + "ATReport_" + dateString + ".xlsx");
        message.Write("Report saved successfully!", true);
        if (calledFromUI == true)
        {
          excelApp.Visible = true;
          Marshal.ReleaseComObject(reportWorkbook);
          Marshal.ReleaseComObject(excelApp);
          reportWorkbook = null;
          excelApp = null;
          return;
        }
        ReleaseExcelApp();
      }
      catch (Exception e)
      {
        message.Write(e.Message, false);
        ReleaseExcelApp();
      }
    }
    public bool RescheduleTests(IMessage message)
    {
      string[] excelFiles = Directory.GetFiles(Directory.GetCurrentDirectory());
      if (excelFiles.Length < 1)
        return false;
      string excelFile = excelFiles.LastOrDefault(f => f.Contains("ATReport.xlsx") == true && f.Contains("$") == false);
      if (excelFile != null)
      {
        Excel.Application pastExcelApp = null;
        bool open;
        try
        {
          pastExcelApp = (Excel.Application)Marshal.GetActiveObject("Excel.Application");
          open = true;
        }
        catch (Exception)
        {
          message.Write("No excel application open", true);

          open = false;
        }
        if (open == false)
          pastExcelApp = new Excel.Application();
        Excel.Workbook pastWorkbook = pastExcelApp.Workbooks.Open(excelFile);
        string exePath = "ScheduleCmd.exe";
        try
        {
          for (int i = 1; i < pastWorkbook.Sheets.Count; i++)
          {
            var pastWorksheet = (Excel.Worksheet)pastWorkbook.Worksheets[i];
            XmlNode atGroup = atReport.SelectSingleNode("ATGroup[@Name='" + pastWorksheet.Name + "']");
            if (atGroup == null)
            {
              message.Write("Could not reschedule tests for " + pastWorksheet.Name, false);
              continue;
            }
            for (int row = 3; row <= pastWorksheet.UsedRange.Rows.Count; row++)
            {
              if (pastWorksheet.Cells[row, columnType.eStatus].Value == "To reschedule")
              {
                string buildLabel = pastWorksheet.Cells[1, 1].Value;
                string testObjectName = pastWorksheet.Cells[row, columnType.eCategory].Value + "\\" + pastWorksheet.Cells[row, columnType.eTest].Value;
                XmlNodeList testScenarios = atGroup.SelectNodes("TestScenario");
                XmlNode testScenario = null;
                foreach (XmlNode testScen in testScenarios)
                {
                  if (testScen.SelectSingleNode("TestObject[@Name='" + testObjectName + "']") != null)
                  {
                    testScenario = testScen;
                    break;
                  }
                }
                if (testScenario == null)
                {
                  message.Write("Could not reschedule " + testObjectName + " on " + pastWorksheet.Name, false);
                  continue;
                }

                string testScenarioID = testScenario.SelectSingleNode("ScenarioID").InnerText;
                string hostName = pastWorksheet.Cells[row, columnType.eHostName].Value;
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.FileName = exePath;
                process.StartInfo.Arguments = $"{testScenarioID} {buildLabel} {hostName}";
                process.Start();
                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                  message.Write("Could not reschedule " + testObjectName + " on " + pastWorksheet.Name, false);
                  continue;
                }
                else
                  message.Write("Rescheduled test " + testObjectName + " on " + pastWorksheet.Name, true);
                pastWorksheet.Cells[row, columnType.eStatus].Value = "Rescheduled";
              }
            }
            Marshal.ReleaseComObject(pastWorksheet);
          }
          pastWorkbook.Save();
        }
        catch (Exception e)
        {
          message.Write(e.Message, false);
        }
        finally
        {
          if (open == false)
          {
            pastWorkbook.Close();
            pastExcelApp.Quit();
          }
        }
        return true;
      }
      return false;
    }
    private void ReleaseExcelApp()
    {
      excelApp.ActiveWorkbook.Saved = true;
      reportWorkbook.Close();
      excelApp.Quit();
      Marshal.ReleaseComObject(reportWorkbook);
      Marshal.ReleaseComObject(excelApp);
      reportWorkbook = null;
      excelApp = null;
    }
  }
}
