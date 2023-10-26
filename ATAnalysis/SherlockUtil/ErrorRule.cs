using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SherlockUtil
{
  public class ErrorRule
  {
    public static readonly string outputPattern = @"\[\d{1,2}\/\d{1,2}\/\d{4}\s\d{1,2}:\d{2}:\d{2}\s(AM|PM)\]";
    public static readonly string runtimeMessagesPattern = @"\d{4},\s(January|February|March|April|May|June|July|August|September|October|November|December)\s\d{1,2}\s\((Monday|Tuesday|Wednesday|Thursday|Friday|Saturday|Sunday)\),\s\d{1,2}h\d{1,2}m\d{1,2}s";
    public static readonly string runtimeMessagesLineStartPattern = @"\d{1,2}\/\d{1,2}\/\d{4}\s\d{1,2}:\d{2}:\d{2}\s(AM|PM)";
    public enum ActionType
    {
      eNone,
      eDateTime,
      eLine,
      eAllFile,
      eUnknown,
      eAllLines
    }
    public ActionType ruleActionType { get; set; }
    private Dictionary<string, char> ruleDictionary { get; set; }
    public ErrorRule(string iniOutputFile, string iniPath, string ruleName)
    {
      ReadWriteINIfile iniUtil = new ReadWriteINIfile(iniPath);
      string[] rule = iniUtil.ReadINI(iniOutputFile, ruleName);
      ruleActionType = (ActionType)int.Parse(rule[rule.Length - 1]);
      ruleDictionary = BuildRule(rule);
    }

    public string FindError(string fileToSearch)
    {
      try
      {
        string[] lines = File.ReadAllLines(fileToSearch);
        string extractedText = "";
        bool found = false;
        switch (ruleActionType)
        {
          case ActionType.eDateTime:
            foreach (string line in lines)
            {

              if (Regex.Match(line, outputPattern).Success || Regex.Match(line, runtimeMessagesPattern).Success)
              {
                if (found == true)
                {
                  return extractedText;
                }
              }
              if (LineIsValid(line, ruleDictionary))
              {
                extractedText += line + "\n";
                extractedText = Regex.Replace(extractedText, outputPattern, "");
                extractedText = Regex.Replace(extractedText, runtimeMessagesLineStartPattern, "");
                found = true;
              }
              else
              {
                if (found == true)
                {
                  extractedText += line + "\n";
                }
              }
            }
            return extractedText;

          case ActionType.eLine:
            foreach (string line in lines)
            {
              if (LineIsValid(line, ruleDictionary))
                extractedText += line;
              return extractedText;
            }
            return extractedText;

          case ActionType.eAllFile:
            extractedText = File.ReadAllText(fileToSearch);
            return extractedText;

          case ActionType.eUnknown:
            foreach (string line in lines)
            {
              if (LineIsValid(line, ruleDictionary))
                extractedText = "Unidentified error occured";
              return extractedText;
            }
            return extractedText;

          case ActionType.eAllLines:
            foreach (string line in lines)
            {
              if (found == true && ruleDictionary.ContainsValue('2'))
              {
                string key = ruleDictionary.Where(x => x.Value == '2').FirstOrDefault().Key;
                string noSpacesLine = line.Replace(" ", "");
                if (noSpacesLine.StartsWith(key) == false)
                {
                  found = false;
                }
                else
                  extractedText += line + "\n";

              }
              if (LineIsValid(line, ruleDictionary))
              {
                extractedText += line + "\n";
                found = true;
              }
            }
            return extractedText;

        }
        return extractedText;
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
        return "";
      }
    }
    private static bool LineIsValid(string line, Dictionary<string, char> rules)
    {
      foreach (var item in rules)
      {
        if (item.Value == '1')
          if (line.Contains(item.Key) == false)
            return false;
        if (item.Value == '0')
          if (line.Contains(item.Key) == true)
            return false;
      }
      return true;
    }
    private static Dictionary<string, char> BuildRule(string[] rule)
    {
      Dictionary<string, char> rules = new Dictionary<string, char>();
      foreach (string item in rule)
      {
        if (item.Length == 1)
          continue;
        if (item.Contains("[") == false)
        {
          rules.Add(item, '1');
          continue;
        }
        rules.Add(item.Substring(0, item.Length - 3), item.ElementAt(item.Length - 2));
      }
      return rules;
    }
  }
}