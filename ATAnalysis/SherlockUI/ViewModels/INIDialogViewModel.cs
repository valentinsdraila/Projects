using SherlockUI.Assets;
using SherlockUI.Models;
using SherlockUtil;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace SherlockUI.ViewModels
{
  class INIDialogViewModel : ViewModelBase
  {
    private ReadWriteINIfile iniUtil = new ReadWriteINIfile(@"..\..\..\Config\Sherlock.ini");
    private ReadWriteINIfile buildsUtil = new ReadWriteINIfile(@"..\..\..\Config\BuildsList.txt");
    private readonly List<string> AvailableATGroups = new List<string>() { "24Debug64EN", "24Setup64EN", "24Debug64SYM", "23Debug64EN", "23Setup64EN", "23Debug64SYM" };
    public ObservableCollection<ListItem> ATGroupList { get; set; }
    public ObservableCollection<string> Responsibles { get; set; }
    public string SelectedResponsible { get; set; }
    public INIDialogViewModel()
    {
      Responsibles = new ObservableCollection<string>() { "msojhp" };
      SelectedResponsible = Responsibles[0];
      ATGroupList = new ObservableCollection<ListItem>();
      string[] usedAtGroups = iniUtil.ReadINI("GlobalSettings", "ATGroups");
      foreach (string atGroup in AvailableATGroups)
      {
        string[] buildsString=buildsUtil.ReadINI(atGroup, "Builds");
        List<string> buildsList = buildsString.ToList();
        iniUtil.WriteINI(atGroup, "ParsingOlderBuild", "0");
        string selectedBuild = buildsList[0];
        if (usedAtGroups.Contains(atGroup))
          ATGroupList.Add(new ListItem(atGroup, true, buildsList, selectedBuild));
        else
          ATGroupList.Add(new ListItem(atGroup, false, buildsList, selectedBuild));
      }
    }
    private ICommand apply;
    public ICommand Apply
    {
      get
      {
        if (apply == null)
          apply = new RelayCommand(ApplyComm);
        return apply;
      }
    }
    public void ApplyComm(object param)
    {
      var enabledAtGroups = ATGroupList.Where(e => e.Enabled);
      string atGroupsString = "";
      foreach(var item in enabledAtGroups)
      {
        iniUtil.WriteINI(item.Name, "Release", item.Name.Substring(0, 2)+"A");
        int currentPos = 2;
        string configuration = "";
        while(Char.IsDigit(item.Name[currentPos])==false)
        {
          configuration += item.Name[currentPos];
          currentPos++;
        }
        iniUtil.WriteINI(item.Name, "Platform", item.Name.Substring(currentPos, 2) + " Bit");
        currentPos += 2;
        if (configuration == "Setup")
          configuration = "_Setup";
        iniUtil.WriteINI(item.Name, "Configuration", configuration);
        iniUtil.WriteINI(item.Name, "Language", item.Name.Substring(currentPos));
        iniUtil.WriteINI(item.Name, "Responsible", SelectedResponsible);
        iniUtil.WriteINI(item.Name, "LastBuildReported", item.SelectedItem); //Selected build
        iniUtil.WriteINI(item.Name, "LastBuildPendingTests", "");
        if (item.parsingOlderString == false)
          iniUtil.WriteINI(item.Name, "ParsingOlderBuild", "0");
        else
          iniUtil.WriteINI(item.Name, "ParsingOlderBuild", "1");
        atGroupsString += item.Name + ",";
      }
      if(atGroupsString.Length>0)
      atGroupsString = atGroupsString.Remove(atGroupsString.Length - 1);
      iniUtil.WriteINI("GlobalSettings", "ATGroups", atGroupsString);
      var disabledAtGroups = ATGroupList.Where(e => e.Enabled == false);
      foreach(var item in disabledAtGroups)
      {
        iniUtil.WriteINI(item.Name, null, null);
      }
      MessageBox.Show("The changes have been applied to Sherlock.ini.");
    }

  }
  
}
