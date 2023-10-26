using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SherlockUI.Models
{
  class ListItem : ViewModels.ViewModelBase
  {
    public string Name { get; set; }
    /// <summary>
    /// If the button is enabled, it means the ATGroup is currently present in the INI file.
    /// </summary>
    private bool enabled;
    public bool Enabled
    {
      get
      {
        return enabled;
      }

      set
      {
        enabled = value;
        OnPropertyChanged(nameof(Enabled));
      }
    }
    private List<string> builds;
    public List<string> Builds
    {
      get
      {
        return builds;
      }

      set
      {
        builds = value;
        OnPropertyChanged(nameof(Builds));
      }
    }
    private string selectedItem;
    public string SelectedItem {
      get {
        return selectedItem;
          }
      set {
        selectedItem = value;
        if (selectedItem != Builds[0])
        parsingOlderString = true;
        OnPropertyChanged(nameof(SelectedItem));
      }
    }

    public bool parsingOlderString { get; set; }
    public ListItem() { }
    public ListItem(string name, bool enabled, List<string> buildsList, string selectedBuild)
    {
      Name = name;
      Enabled = enabled;
      Builds = buildsList;
      SelectedItem = selectedBuild;
      parsingOlderString = false;
    }
  }
}
