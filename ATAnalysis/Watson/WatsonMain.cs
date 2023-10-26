using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Xml;
using SherlockUtil;

namespace Watson
{
  class WatsonMain
  {
    public static string FormatBeginTime(string beginTime)
    {
      beginTime = beginTime.Replace(',', '-');
      beginTime = beginTime.Replace(" ", "");
      int index = 0;
      string formattedTime = "";
      while (beginTime[index] != '-')
      {
        formattedTime += beginTime[index];
        index++;
      }
      while (beginTime[index] != '0' && beginTime[index] != '1' && beginTime[index] != '2' && beginTime[index] != '3')
      {
        formattedTime += beginTime[index];
        index++;
      }
      formattedTime += '-';
      while (beginTime[index] != '(')
      {
        formattedTime += beginTime[index];
        index++;
      }
      index++;
      formattedTime += ',';
      while (beginTime[index] != ')')
      {
        formattedTime += beginTime[index];
        index++;
      }
      return formattedTime;

    }

    static void Main(string[] args)
    {
      ReadWriteINIfile iniUtil = new ReadWriteINIfile(@"..\..\..\Config\Sherlock.ini");
      if (args.Length > 1)
        iniUtil.path = args[1];
      string[] atGroups = iniUtil.ReadINI("GlobalSettings", "ATGroups");
      XmlDocument xmlOutput = new XmlDocument();
      XmlDeclaration declaration = xmlOutput.CreateXmlDeclaration("1.0", "UTF-8", null);
      xmlOutput.AppendChild(declaration);
      int scenarioCount = 0;
      int testObjectCount = 0;

      XmlElement atReport = xmlOutput.CreateElement("ATReport");
      atReport.SetAttribute("Version", "1.0");
      xmlOutput.AppendChild(atReport);

      foreach (var item in atGroups)
      {
        ATGroup atGroup = new ATGroup();
        var groupName = item;
        atGroup.Name = groupName;
        atGroup.Release = iniUtil.ReadINI(item, "Release")[0];
        atGroup.Platform = iniUtil.ReadINI(item, "Platform")[0];
        atGroup.Configuration = iniUtil.ReadINI(item, "Configuration")[0];
        atGroup.Language = iniUtil.ReadINI(item, "Language")[0];
        atGroup.Responsible = iniUtil.ReadINI(item, "Responsible")[0];
        atGroup.LastBuildPendingTests = iniUtil.ReadINI(item, "LastBuildPendingTests")[0];
        if (atGroup.CheckATGroup() == true)
        {
          atGroup.LastBuildReported = atGroup.GetLatestBuild();
          iniUtil.WriteINI(atGroup.Name, "LastBuildReported", atGroup.LastBuildReported);
          string connectionString = "Data Source=server1;Initial Catalog=Testing;Integrated Security=True;";
          using (SqlConnection cnn2 = new SqlConnection(connectionString))
          {
            try
            {
              cnn2.Open();
            }
            catch (Exception)
            {
              Console.WriteLine("Can not open connection ! ");
            }
            string queryString = "SELECT TestObject.Responsible, TestObjectRuntime.EndTime, TestScenarioRuntime.ScenarioRuntimeID, TestObjectRuntime.BeginTime, TestScenarioRuntime.Context, TestScenario.ScenarioID, TestObject.TestObjectID," +
           " TestScenarioRuntime.InputPath, TestScenarioRuntime.OutputPath, TestScenarioRuntime.UsedFrontend, TestScenarioRuntime.HostName, TestObjectRuntime.TestObjectRuntimeID, TestScenario.Responsible, TestObjectRuntime.Status"
       + " FROM TestScenarioRuntime INNER JOIN TestScenario ON TestScenario.ScenarioID = TestScenarioRuntime.ScenarioID INNER JOIN TestObjectRuntime ON TestObjectRuntime.ScenarioRuntimeID = TestScenarioRuntime.ScenarioRuntimeID INNER JOIN TestObject ON TestObject.TestObjectID = TestObjectRuntime.ObjectID " +
       "WHERE TestScenarioRuntime.Context = @LastBuildReported AND TestScenarioRuntime.Language=@Language" +
      " AND (TestObject.Responsible LIKE @Responsible) AND NOT TestObjectRuntime.Status='NULL' AND ( CASE WHEN" +
                "  TestObjectRuntime.Status like 'ERROR%' THEN  1 ELSE CASE WHEN 	(Select COUNT(TestObjectRuntimeMessages.Message)" +
                " FROM TestObjectRuntimeMessages WHERE(TestObjectRuntimeMessages.TestObjectRuntimeID = TestObjectRuntime.TestObjectRuntimeID) AND " +
                "(TestObjectRuntimeMessages.Status = 'ERROR' )  ) > 0 THEN 1 ELSE CASE WHEN (Select COUNT(TestObjectRuntimeMessages.Message)" +
                " FROM TestObjectRuntimeMessages WHERE(TestObjectRuntimeMessages.TestObjectRuntimeID = TestObjectRuntime.TestObjectRuntimeID)) = 0 THEN 1 ELSE 0  End End End  = 1)"
    + " AND CONVERT(datetime, SubString(TestObjectRuntime.BeginTime, CHARINDEX ( '(',TestObjectRuntime.BeginTime) - 3, 2) + ' ' +SubString(TestObjectRuntime.BeginTime, 7, 3) + ' ' +  SubString(TestObjectRuntime.BeginTime, 3, 2) + ' ' + SubString(RIGHT(TestObjectRuntime.BeginTime,9), 1, 2)+ ':' + SubString(RIGHT(TestObjectRuntime.BeginTime,8), 3, 2)+ ':' + SubString(RIGHT(TestObjectRuntime.BeginTime,8),6 , 2)+ ':000', 113)" +
    "=( SELECT MAX(  CONVERT(datetime, SubString(Tor.BeginTime, CHARINDEX ( '(',Tor.BeginTime) - 3, 2) + ' ' +SubString(Tor.BeginTime, 7, 3) + ' ' +  SubString(Tor.BeginTime, 3, 2) + ' ' + SubString(RIGHT(Tor.BeginTime,9), 1, 2)+ ':' + SubString(RIGHT(Tor.BeginTime,8), 3, 2)+ ':' + SubString(RIGHT(Tor.BeginTime,8),6 , 2)+ ':000', 113)) FROM TestScenarioRuntime Rntm" +
    " INNER JOIN TestObjectRuntime Tor ON Tor.ScenarioRuntimeID=Rntm.ScenarioRuntimeID WHERE Rntm.Context=@LastBuildReported AND Tor.ObjectID=TestObject.TestObjectID AND Rntm.UsedFrontend=TestScenarioRuntime.UsedFrontend AND Rntm.Language=@Language)";

            using (SqlCommand command = new SqlCommand(queryString, cnn2))
            {
              try
              {
                XmlElement XmlATGroup = xmlOutput.CreateElement("ATGroup");
                XmlATGroup.SetAttribute("Name", atGroup.Name);
                atReport.AppendChild(XmlATGroup);

                XmlElement build = xmlOutput.CreateElement("LastBuildReported");
                build.InnerText = atGroup.LastBuildReported;
                XmlATGroup.AppendChild(build);

                List<XmlElement> testScenarioList = new List<XmlElement>();

                command.Parameters.AddWithValue("@LastBuildReported", atGroup.LastBuildReported);
                command.Parameters.AddWithValue("@Language", atGroup.Language);
                command.Parameters.AddWithValue("@Responsible", "%" + atGroup.Responsible + "%");
                using (var reader = command.ExecuteReader())
                {
                  while (reader.Read())
                  {
                    XmlElement testScen = xmlOutput.CreateElement("TestScenario");
                    string resp = (string)reader[0];
                    string frontendString = (string)reader["UsedFrontend"];
                    string truncateTestScenario = (string)reader["ScenarioID"];
                    string testScenName = truncateTestScenario.Replace("TestMembers\\TestScenarios\\", "");
                    testScen.SetAttribute("Name", testScenName);
                    XmlElement testScenario = testScenarioList.FirstOrDefault(ts => ts.Attributes.GetNamedItem("Name").Value == testScen.Attributes.GetNamedItem("Name").Value);
                    if (testScenario == null || (testScenario!=null && testScenario.SelectSingleNode("FrontEnd").Value !=frontendString))
                    {
                      testScenario = testScen;
                      testScenarioList.Add(testScenario);
                    }

                    testObjectCount++;
                    XmlElement testObject = xmlOutput.CreateElement("TestObject");
                    string truncateTestObject = (string)reader["TestObjectID"];
                    testObject.SetAttribute("Name", truncateTestObject.Replace("TestMembers\\TestObjects\\", ""));
                    testScenario.AppendChild(testObject);
                    XmlElement inputFolder = xmlOutput.CreateElement("InputFolder");
                    inputFolder.InnerText = @"\\server1" + "\\AutomatedTestingVOB\\" + atGroup.Release + "\\" + (string)reader["TestObjectID"] + "\\Input";
                    testObject.AppendChild(inputFolder);

                    XmlElement outputFolder = xmlOutput.CreateElement("OutputFolder");
                    string outputPath = (string)reader["OutputPath"];
                    string hostname = (string)reader["HostName"];
                    string beginTime = (string)reader["BeginTime"];
                    string status = (string)reader["Status"];
                    beginTime = FormatBeginTime(beginTime);
                    outputFolder.InnerText = "\\\\" + hostname.ToUpper() + outputPath.Replace("D:", "") + "\\" + beginTime + "\\"
                    + hostname.ToUpper() + "\\ScenarioRuntimeID-" + reader["ScenarioRuntimeID"].ToString() + "\\TestObjectRuntimeID-" + reader["TestObjectRuntimeID"].ToString();
                    testObject.AppendChild(outputFolder);

                    XmlElement testObjectStatus = xmlOutput.CreateElement("Status");
                    testObjectStatus.InnerText = status;
                    testObject.AppendChild(testObjectStatus);

                    XmlElement frontend = xmlOutput.CreateElement("FrontEnd");
                    frontend.InnerText = frontendString;
                    testScenario.AppendChild(frontend);

                    XmlElement hostName = xmlOutput.CreateElement("HostName");
                    hostName.InnerText = hostname;
                    testScenario.AppendChild(hostName);

                    string connectionStringPortal = "Data Source=server1;Initial Catalog=Testing;Integrated Security=True;";
                    using (SqlConnection cnnPortal = new SqlConnection(connectionStringPortal))
                    {
                      try
                      {
                        cnnPortal.Open();
                      }
                      catch (Exception)
                      {
                        Console.WriteLine("Can not open connection ! ");
                      }

                      string revision = atGroup.Release;
                      string fashion = "";
                      if (atGroup.Configuration == "Debug")
                        fashion = "Debug";
                      else
                        fashion = "_setup";
                      string query = "SELECT TOP 1 spl.[ID]" +
                    ", CASE WHEN t.FrontEnd IN('NO FRONTEND', 'NO FRONTEND INST') THEN NULL ELSE t.Name END AS TestSystemName, [Fashion], [Platform] FROM[Portal].[dbo].[ScenarioProductLink] spl" +
                    " INNER JOIN[Portal].[dbo].[TestSystems] t ON spl.FrontEnd = t.FrontEnd" +
                    " FULL OUTER JOIN[Portal].[dbo].[Product] p ON spl.ProductID = p.ProductID" +
                    " LEFT OUTER JOIN[Portal].[dbo].[Scenarios] s ON spl.ScenarioName = s.ScenarioName" +
                    " WHERE p.ProductName = 'TL' AND spl.[ScenarioName] ='" + testScenName + "' AND spl.[FrontEnd]='" + frontendString + "' AND p.Revision = '" + revision + "' AND Fashion = '" + fashion + "' AND Platform = 'intel_amd64_winxp' ORDER BY spl.[ID] DESC";

                      using (SqlCommand portalcommand = new SqlCommand(query, cnnPortal))
                      {
                        try
                        {
                          using (var readerPortal = portalcommand.ExecuteReader())
                          {
                            while (readerPortal.Read())
                            {
                              int ID = (int)readerPortal[0];
                              XmlNode scenarioID = xmlOutput.CreateElement("ScenarioID");
                              scenarioID.InnerText = ID.ToString();
                              testScenario.AppendChild(scenarioID);
                            }
                          }
                        }
                        catch (Exception e)
                        {
                          Console.WriteLine(e.Message);
                        }
                      }
                      cnnPortal.Close();
                    }
                  }
                  foreach (XmlElement testScenario in testScenarioList)
                  {
                    XmlATGroup.AppendChild(testScenario);
                  }
                  scenarioCount += testScenarioList.Count;

                 
                }

              }
              catch (Exception e)
              {
                Console.WriteLine(e.Message);
                Console.WriteLine("Execution failed!");
              }
            }
            cnn2.Close();
          }
        }
        else
        {
          Console.WriteLine("The ATGroup parameters are not valid!");
        }
      }
      if (args.Length > 0)
      {
        XmlElement testScenarioCount = xmlOutput.CreateElement("ScenarioCount");
        testScenarioCount.InnerText = scenarioCount.ToString();
        atReport.AppendChild(testScenarioCount);
        XmlElement testObjCount = xmlOutput.CreateElement("ObjectCount");
        testObjCount.InnerText = testObjectCount.ToString();
        atReport.AppendChild(testObjCount);
        xmlOutput.Save(args[0]);
      }
      else
      {
        XmlElement testScenarioCount = xmlOutput.CreateElement("ScenarioCount");
        testScenarioCount.InnerText = scenarioCount.ToString();
        atReport.AppendChild(testScenarioCount);
        XmlElement testObjCount = xmlOutput.CreateElement("ObjectCount");
        testObjCount.InnerText = testObjectCount.ToString();
        atReport.AppendChild(testObjCount);
        xmlOutput.Save("xmlOutput.xml");
      }
    }
  }
}
