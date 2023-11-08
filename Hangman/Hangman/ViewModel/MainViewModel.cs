using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Hangman.View;
using Hangman.Commands;
using System.ComponentModel;
using System.Windows;
using System.IO;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using Hangman.Model;
using System.Reflection;

namespace Hangman.ViewModel
{
    class MainViewModel : ViewModelBase
    {
        private static ObservableCollection<string> names = new ObservableCollection<string>();
        private bool enableButton;
        public bool EnableButton 
        { 
        get {
                return enableButton;
            } 
            set
            {
                enableButton = value;
                SetPropertyChanged(nameof(EnableButton));
            }
        }
        public static ObservableCollection<Users> Users { get; set; }
        public MainViewModel()
        {
            if(SelectedUser==null)
            EnableButton = false;
            using (StreamReader r = new StreamReader("json.json"))
            {
                string json = r.ReadToEnd();
                if (json != "")
                {
                    Users = JsonConvert.DeserializeObject<ObservableCollection<Users>>(json);
                    foreach (var item in Users)
                    {
                        names.Add(item.username);
                    }
                }
            }
        }
        private string selectedName;
        public string SelectedName { get
            {
                return selectedName;
            }
            set
            {
                selectedName = value;
                SetPropertyChanged("SelectedName");
            }
        }
        private Users selectedUser;
        public Users SelectedUser
        {
            get
            {
                return selectedUser;
            }
            set
            {
                EnableButton = true;
                selectedUser = value;
                SetPropertyChanged(nameof(SelectedUser));
                SetPropertyChanged(nameof(EnableButton));
            }
        }
        public static ObservableCollection<string> Names
        {
            get
            {
                return names;
            }
        }
        private ICommand newCommand;
        public ICommand NewCommand
        {
            get
            {
                if (newCommand == null)
                    newCommand = new RelayCommand(NewUser);
                return newCommand;
            }
        }
        public void NewUser(object parameter)
        {
            NewDialog n = new NewDialog();
            n.ShowDialog();
            SetPropertyChanged("New");
        }
        private ICommand deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                if (deleteCommand == null)
                    deleteCommand = new RelayCommand(DeleteUser);
                return deleteCommand;
            }
        }
        private ICommand playCommand;
        public ICommand PlayCommand
        {
            get
            {
                if (playCommand == null)
                    playCommand = new RelayCommand(PlayComm);
                return playCommand;
            }
        }
        public void PlayComm(object parameter)
        {
            var g = new GameWindow();
            g.ShowDialog();
        }
        public void DeleteUser(object parameter)
        {
            for (int i = 0; i < Users.Count; i++)
            {
                if (Users[i] == SelectedUser)
                    Users.Remove(Users[i]);
            }
            names.Remove(SelectedName);
            
            string json = JsonConvert.SerializeObject(Users.ToArray());
            System.IO.File.WriteAllText("json.json", json);
            SetPropertyChanged("DeleteCommand");
        }
        
    }
}
