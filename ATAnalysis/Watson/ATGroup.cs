using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using SherlockUtil;

namespace Watson
{
  class ATGroup
  {
    public string Name { get; set; }
    public string Release { get; set; }
    public string Platform { get; set; }
    public string Configuration { get; set; }
    public string Language { get; set; }
    public string Responsible { get; set; }
    public string LastBuildReported { get; set; }
    public string LastBuildPendingTests { get; set; }
    public ReadWriteINIfile iniUtil = new ReadWriteINIfile(@"..\..\..\Config\Sherlock.ini");

    public bool CheckATGroup()
    {
      Type type = this.GetType();
      foreach (var property in type.GetProperties())
      {
        if (property.Name == "Name")
        {
          continue;
        }
        if (property.Name == "LastBuildReported")
          break;
        if (iniUtil.ReadINI("GlobalSettings", property.Name + "s").Contains(property.GetValue(this)) == false)
        {
          Console.WriteLine("ATGroup parameters are not valid!");
          var test = iniUtil.ReadINI("GlobalSettings", property.Name + "s");
        }
      }
      return true;
    }

    public string GetLatestBuild()
    {
      List<string> buildsList = new List<string>();
      if (Configuration == "Debug")
      {
        string bitSuffix;
        if (Platform == "64 Bit")
        {
          bitSuffix = "_64";
        }
        else
        {
          bitSuffix = "_32";
        }
        string config = Configuration.ToLower();
        string connectionString = "Data Source=server1;Initial Catalog=Testing;Integrated Security=True;";
        using (var cnn2 = new SqlConnection(connectionString))
        {
          try
          {
            cnn2.Open();
          }
          catch (Exception)
          {
            Console.WriteLine("Cannot open connection!");
            throw;
          }

          string queryString = "";

          queryString = "SELECT distinct TOP 10 Context FROM TestScenarioRuntime WHERE (Context LIKE @Context1 OR Context LIKE @Context2)" +
                                     "       AND ( not Context Like '%_Sandbox_%') AND Language = @Language ORDER BY Context DESC";

          var command = new SqlCommand(queryString, cnn2);
          command.Parameters.AddWithValue("@Context1", $"[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]_{Release}.[0-9][0-9][0-9][0-9][0-9][0-9]_[0-9][0-9][0-9][0-9].[0-9]_{config}{bitSuffix}_en");
          command.Parameters.AddWithValue("@Context2", $"[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]_{Release}.[0-9][0-9][0-9][0-9][0-9][0-9]_[0-9][0-9][0-9][0-9].[0-9][0-9]_{config}{bitSuffix}_en");
          command.Parameters.AddWithValue("@Language", Language);
          using (var reader = command.ExecuteReader())
          {
            while (reader.Read())
              buildsList.Add((string)reader["Context"]);
          }
        }

      }
      if (Configuration == "_Setup")
      {
        string platform = Platform.Remove(2, 1);
        platform = "_" + platform;
        if (platform == "_32Bit")
          platform = "";
        string connectionString = "Data Source=server1;Initial Catalog=Testing;Integrated Security=True;";
        using (var cnn2 = new SqlConnection(connectionString))
        {
          try
          {
            cnn2.Open();
          }
          catch (Exception)
          {
            Console.WriteLine("Cannot open connection!");
            throw;
          }

          string queryString = "";
          queryString = "SELECT distinct TOP 10 CONTEXT FROM TestScenarioRuntime WHERE " + $@"(Context LIKE '{Release}.[0-9][0-9][0-9][0-9][0-9][0-9]_en{platform}') AND ( not Context Like '%_Sandbox_%') AND (Language = '{Language.ToLower()}')" + " ORDER BY Context DESC";
          var command = new SqlCommand(queryString, cnn2);
          using (var reader = command.ExecuteReader())
          {
            while (reader.Read())
              buildsList.Add((string)reader["Context"]);
          }
        }
      }
      bool olderBuild = false;
      try
      {
        olderBuild = int.Parse(iniUtil.ReadINI(Name, "ParsingOlderBuild")[0])!=0;
      }
      catch(Exception e)
      {
        Console.WriteLine(e.Message);
      }

      ReadWriteINIfile buildUtil = new ReadWriteINIfile(@"..\..\..\Config\BuildsList.txt");
      string BuildsString = "";
      foreach (string build in buildsList)
        BuildsString += build + ",";
      buildUtil.WriteINI(Name, "Builds", BuildsString);

      if (olderBuild)
      {
        iniUtil.WriteINI(Name, "ParsingOlderBuild", "0");
        return iniUtil.ReadINI(Name, "LastBuildReported")[0];
      }

      else
        return buildsList[0];
    }
  }
}
