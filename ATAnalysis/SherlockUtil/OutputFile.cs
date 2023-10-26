using System.Collections.Generic;

namespace SherlockUtil
{
  public class OutputFile
  {
    public string Name { get; set; }
    public string Path { get; set; }
    public List<ErrorRule> errorRules { get; set; }

    public OutputFile() { }

    public OutputFile(string name, string iniPath, string path)
    {
      Name = name;
      ReadWriteINIfile iniUtil = new ReadWriteINIfile(iniPath);
      Path = path;
      errorRules = new List<ErrorRule>();
      List<string> rules = iniUtil.GetKeys(name);
      if (rules.Count > 0)
        foreach (var rule in rules)
          errorRules.Add(new ErrorRule(name, iniPath, rule));
    }
    public string ExtractError()
    {
      string errorFound = "";
      foreach (var rule in errorRules)
      {
        errorFound = rule.FindError(Path);
        if (errorFound != "")
          return errorFound;
      }
      return errorFound;
    }
  }
}
