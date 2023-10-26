using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace SherlockUtil
{
  public class ReadWriteINIfile
  {
    [DllImport("kernel32")]
    private static extern long WritePrivateProfileString(string name, string key, string val, string filePath);
    [DllImport("kernel32")]
    private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
    [DllImport("kernel32.dll")]
    private static extern int GetPrivateProfileSection(string lpAppName, byte[] lpszReturnBuffer, int nSize, string lpFileName);

    public string path;

    public ReadWriteINIfile(string inipath)
    {
      path = inipath;
    }
    public void WriteINI(string name, string key, string value)
    {
      WritePrivateProfileString(name, key, value, this.path);
    }
    public List<string> GetKeys(string category)
    {

      byte[] buffer = new byte[2048];

      GetPrivateProfileSection(category, buffer, 2048, path);
      String[] tmp = Encoding.ASCII.GetString(buffer).Trim('\0').Split('\0');

      List<string> result = new List<string>();
      if (tmp.Length == 1 && tmp[0] == "")
        return result;
      foreach (String entry in tmp)
      {
        try
        {
          result.Add(entry.Substring(0, entry.IndexOf("=")));
        }
        catch (Exception)
        {
          Console.WriteLine(category);
        }
      }

      return result;
    }
    public string[] ReadINI(string name, string key)
    {
      StringBuilder sb = new StringBuilder(255);
      int ini = GetPrivateProfileString(name, key, "", sb, 255, this.path);
      string readResult = sb.ToString();
      if (readResult.Contains(",") == true)
      {
        string[] returnedArray = readResult.Split(',');
        return returnedArray;
      }
      string[] returned = new string[1];
      returned[0] = readResult;
      return returned;
    }

    public void CreateINIFile()
    {
      if (File.Exists("Sherlock.ini"))
        File.Delete("Sherlock.ini");
      List<string> AvailableATGroupList = new List<string>() { "23Debug64EN", "22Debug64EN", "22Setup64EN", "23Setup64EN", "22Setup32EN", "22Debug32EN" };
      List<string> AvailableReleasesList = new List<string>() { "22A", "23A" };
      List<string> AvailableConfigurationsList = new List<string>() { "Debug", "DebugRelease", "_Setup" };
      List<string> AvailableLanguagesList = new List<string>() { "EN", "SYM" };
      List<string> AvailableATGroupResponsibles = new List<string>() { "msojhp" };
      List<string> AvailablePlatformsList = new List<string>() { "64 Bit", "32 Bit" };

      List<string> UserATGroupList = new List<string>();
      List<string> UserReleasesList = new List<string>();
      List<string> UserConfigurationsList = new List<string>();
      List<string> UserLanguagesList = new List<string>();
      List<string> UserResponsiblesList = new List<string>();
      List<string> UserPlatformsList = new List<string>();

      Console.WriteLine("Choose the releases you are interested in, type 'N' when you are done.");
      string read = "";
      while (read != "N")
      {
        Console.WriteLine("Available releases:");

        foreach (string item in AvailableReleasesList)
          Console.Write(item + " ");
        Console.WriteLine();

        read = Console.ReadLine();
        if (AvailableReleasesList.Contains(read))
        {
          AvailableReleasesList.Remove(read);
          UserReleasesList.Add(read);
        }
      }

      Console.WriteLine("Choose the platforms you are interested in, type 'N' when you are done.");
      read = "";
      while (read != "N")
      {
        Console.WriteLine("Available platforms:");
        foreach (string item in AvailablePlatformsList)
          Console.Write(item + " ");
        Console.WriteLine();

        read = Console.ReadLine();
        if (AvailablePlatformsList.Contains(read))
        {
          AvailablePlatformsList.Remove(read);
          UserPlatformsList.Add(read);
        }
      }

      Console.WriteLine("Choose the configurations you are interested in, type 'N' when you are done.");
      read = "";
      while (read != "N")
      {
        Console.WriteLine("Available configurations:");

        foreach (string item in AvailableConfigurationsList)
          Console.Write(item + " ");
        Console.WriteLine();

        read = Console.ReadLine();
        if (AvailableConfigurationsList.Contains(read))
        {
          AvailableConfigurationsList.Remove(read);
          UserConfigurationsList.Add(read);
        }
      }

      Console.WriteLine("Choose the languages you are interested in, type 'N' when you are done.");
      read = "";
      while (read != "N")
      {
        Console.WriteLine("Available languages:");

        foreach (string item in AvailableLanguagesList)
          Console.Write(item + " ");
        Console.WriteLine();

        read = Console.ReadLine();
        if (AvailableLanguagesList.Contains(read))
        {
          AvailableLanguagesList.Remove(read);
          UserLanguagesList.Add(read);
        }
      }

      Console.WriteLine("Choose the ATGroups you are interested in, type 'N' when you are done.");
      read = "";
      while (read != "N")
      {
        Console.WriteLine("Available ATGroups:");

        foreach (string item in AvailableATGroupList)
          Console.Write(item + " ");
        Console.WriteLine();

        read = Console.ReadLine();
        if (AvailableATGroupList.Contains(read))
        {
          AvailableATGroupList.Remove(read);
          UserATGroupList.Add(read);
        }
      }

      Console.WriteLine("Choose the AT group responsibles you are interested in, type 'N' when you are done.");
      read = "";
      while (read != "N")
      {
        Console.WriteLine("Available AT group responsibles:");

        foreach (string item in AvailableATGroupResponsibles)
          Console.Write(item + " ");
        Console.WriteLine();

        read = Console.ReadLine();
        if (AvailableATGroupResponsibles.Contains(read))
        {
          AvailableATGroupResponsibles.Remove(read);
          UserResponsiblesList.Add(read);
        }
      }
      string watsonOutputPath = "WatsonOutput.xml";

      StringBuilder sb = new StringBuilder();
      sb.AppendLine("[GlobalSettings]");
      sb.Append("Releases=");
      for (int i = 0; i < UserReleasesList.Count - 1; i++)
      {
        sb.Append(UserReleasesList[i]);
        sb.Append(",");
      }
      sb.Append(UserReleasesList[UserReleasesList.Count - 1]);
      sb.AppendLine();
      sb.AppendLine("; All supported releases e.g. Releases=23A,22A,… ");

      sb.Append("Platforms=");
      for (int i = 0; i < UserPlatformsList.Count - 1; i++)
      {
        sb.Append(UserPlatformsList[i]);
        sb.Append(",");
      }
      sb.Append(UserPlatformsList[UserPlatformsList.Count - 1]);
      sb.AppendLine();
      sb.AppendLine("; All supported platforms");

      sb.Append("Configurations=");
      for (int i = 0; i < UserConfigurationsList.Count - 1; i++)
      {
        sb.Append(UserConfigurationsList[i]);
        sb.Append(",");
      }
      sb.Append(UserConfigurationsList[UserConfigurationsList.Count - 1]);
      sb.AppendLine();
      sb.AppendLine("; All supported configurations");

      sb.Append("Languages=");
      for (int i = 0; i < UserLanguagesList.Count - 1; i++)
      {
        sb.Append(UserLanguagesList[i]);
        sb.Append(",");
      }
      sb.Append(UserLanguagesList[UserLanguagesList.Count - 1]);
      sb.AppendLine();
      sb.AppendLine("; All supported languages e.g. Languages=EN,SYM ");

      sb.Append("Responsibles=");
      for (int i = 0; i < UserResponsiblesList.Count - 1; i++)
      {
        sb.Append(UserResponsiblesList[i]);
        sb.Append(",");
      }
      sb.Append(UserResponsiblesList[UserResponsiblesList.Count - 1]);
      sb.AppendLine();
      sb.AppendLine("; List of AT responsibles");

      sb.Append("ATGroups=");
      for (int i = 0; i < UserATGroupList.Count - 1; i++)
      {
        sb.Append(UserATGroupList[i]);
        sb.Append(",");
      }
      sb.Append(UserATGroupList[UserATGroupList.Count - 1]);
      sb.AppendLine();
      sb.AppendLine("; Names of the AT groups for which a report will be generated. Each name corresponds with a separate section in the ini file. ");
      sb.AppendLine("; For each item listed in the ATGroups setting in the GlobalSettings section, a separate section with the same name has to be made.");

      sb.AppendLine("WatsonOutputPath=" + watsonOutputPath);
      sb.AppendLine();

      foreach (string ATGroup in UserATGroupList)
      {
        sb.AppendLine("[" + ATGroup + "]");
        sb.Append("Release=");
        string ATGroupRelease = ATGroup.Substring(0, 2);
        foreach (string release in UserReleasesList)
        {
          if (release.Contains(ATGroupRelease))
            sb.Append(release);
        }
        sb.AppendLine();

        sb.Append("Configuration=");
        string ATGroupConfig = "";
        int position = 2;
        while (position < ATGroup.Length)
        {
          if (Char.IsDigit(ATGroup[position]))
            break;
          ATGroupConfig += ATGroup[position];
          position++;
        }
        foreach (string config in UserConfigurationsList)
        {
          if (config.Contains(ATGroupConfig) && config.Length <= ATGroupConfig.Length + 1)
            sb.Append(config);
        }
        sb.AppendLine();

        sb.Append("Platform=");
        string ATGroupPlatform = ATGroup.Substring(position, 2);
        position += 2;
        foreach (string platform in UserPlatformsList)
        {
          if (platform.Contains(ATGroupPlatform))
            sb.Append(platform);
        }
        sb.AppendLine();


        sb.Append("Language=");
        string ATGroupLanguage = ATGroup.Substring(position);
        foreach (string language in UserLanguagesList)
        {
          if (language.Contains(ATGroupLanguage))
            sb.Append(language);
        }
        sb.AppendLine();

        sb.AppendLine("Responsible=" + UserResponsiblesList[0]);
        sb.AppendLine("LastBuildReported=");
        sb.AppendLine("LastBuildPendingTests=");
        sb.AppendLine();
      }
      sb.AppendLine("[ATOutputFiles]");
      sb.AppendLine("; The list of test output files to examine. Each item will have following format: ");
      sb.AppendLine(@"Output=ATFSystem\output.txt");
      sb.AppendLine(@"RuntimeMessages=runtimeMessages.txt");
      sb.AppendLine(@"LogError=ATFSystem\error.txt");
      sb.AppendLine(@"StdError=ATFSystem\stderr_*.txt");
      sb.AppendLine(@"ErrorPicture=ATFSystem\1.jpg");

      sb.AppendLine("; For each item in ATOutputFiles an associated section must be made.");
      sb.AppendLine("; It will contain the keywords that have to be searched for in this file, together with the action that should be taken on it.");
      sb.AppendLine("; So the syntax is: ");
      sb.AppendLine("; {Rule}={Keyword1, Keyword2},{Action}");
      sb.AppendLine(";   Action = a number corresponding with an action. Currently following actions are defined: ");
      sb.AppendLine(";   1 = Extract the text part of the log file where the keyword was found, starting from the preceding datetime tag and until the next datetime tag encountered. ");
      sb.AppendLine(";   2 = Extract only the line in which the keyword was found  ");
      sb.AppendLine(";   3 = Take entire content of the file");
      sb.AppendLine(";   4 = In case this keyword is present and no other known anomaly was encountered during the parsing process, ");
      sb.AppendLine(";       it means the anomaly is not a known one yet and slips through the mazes of the keywords web.  ");
      sb.AppendLine(";       In this case, the message that must be composed for the output is “Unidentified error occurred.");
      sb.AppendLine(";   5 = Extract all the lines where the keyword was found");
      sb.AppendLine(";   Default action is 1. ");

      sb.AppendLine();
      sb.AppendLine("[Output]");
      sb.AppendLine(";Error=1 ");
      sb.AppendLine(";Exception=2");
      sb.AppendLine(";Differences_found=1");
      sb.AppendLine(";Nothing_to_compare=2");
      sb.AppendLine(";FAILED=4");
      sb.AppendLine("Rule1=Differences found,1");
      sb.AppendLine("Rule2=ERROR[1],RESULT[0],1");
      sb.AppendLine("Rule3=Exception[1],at[2],5");
      sb.AppendLine("Rule4=Nothing to compare,2");
      sb.AppendLine("Rule5=Failed,4");
      sb.AppendLine(";0 = Does not contain keyword");
      sb.AppendLine(";1 = contains keyword");
      sb.AppendLine(";2 = optional(for further parsing)");
      sb.AppendLine();
      sb.AppendLine("[RuntimeMessages]");
      sb.AppendLine("Rule1=Differences found,1");
      sb.AppendLine("Rule2=ERROR[1],RESULT[0],1");
      sb.AppendLine("Rule3=Exception[1],at[2],5");
      sb.AppendLine("Rule4=Nothing to compare,2");
      sb.AppendLine("Rule5=Failed,4");
      sb.AppendLine();
      sb.AppendLine("[LogError] ");
      sb.AppendLine("Rule1=All,3");
      sb.AppendLine();
      sb.AppendLine("[StdError] ");
      sb.AppendLine("Rule1=All,3");

      string text = sb.ToString();

      File.AppendAllText("Sherlock.ini", text);
    }
  }
}
