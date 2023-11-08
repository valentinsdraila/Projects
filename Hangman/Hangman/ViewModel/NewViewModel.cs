using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Hangman.View;
using Hangman.Commands;
using System.IO;
using Newtonsoft.Json;
using System.Windows;
using Hangman.Model;
using System.Collections.ObjectModel;

namespace Hangman.ViewModel
{
    [Serializable]
    class NewViewModel : ViewModelBase
    {
        private string text;
        public string Text
        {
            get { return this.text; }
            set
            {
                if (!string.Equals(this.text, value))
                {
                    this.text = value;
                    OnPropertyChanged("Text");
                }
            }
        }
        private string selectedImage;
        public string SelectedImage
        {
            get { return this.selectedImage; }
            set
            {
                if (!string.Equals(this.selectedImage, value))
                {
                    this.selectedImage = value;
                    OnPropertyChanged("SelectedImage");
                }
            }
        }
        public ObservableCollection<string> NewDialogImages { get; set; }
        public NewViewModel()
        {
            NewDialogImages = new ObservableCollection<string>();
            NewDialogImages.Add(AppDomain.CurrentDomain.BaseDirectory+"image1.png");
            NewDialogImages.Add(AppDomain.CurrentDomain.BaseDirectory + "image2.png");
            NewDialogImages.Add(AppDomain.CurrentDomain.BaseDirectory + "image3.png");
            NewDialogImages.Add(AppDomain.CurrentDomain.BaseDirectory + "image4.png");
            
        }
        private void RaisePropertyChanged()
        {
            throw new NotImplementedException();
        }

        private ICommand addNewCommand;
        public ICommand AddNewCommand
        {
            get
            {
                if (addNewCommand == null)
                    addNewCommand = new RelayCommand(AddNewUser);
                return addNewCommand;
            }
        }
        public void AddNewUser(object parameter)
        {
            Users user = new Users();
            user.username = text;
            user.image = SelectedImage;
            List < Users> l= new List<Users>();
            using (StreamReader r = new StreamReader("json.json"))
            {
                string json1 = r.ReadToEnd();
                if (json1 != "")
                {
                 l = JsonConvert.DeserializeObject<List<Users>>(json1);
                }
            }
            bool containsItem = l.Any(item => item.username == user.username);
            if (containsItem)
            {
                MessageBox.Show("User already exists!");
            }
            else
            {
                l.Add(user);
                MainViewModel.Users.Add(user);
                MessageBox.Show("User added!");
            }
            string json = JsonConvert.SerializeObject(l.ToArray());
            System.IO.File.WriteAllText("json.json", json);
            SetPropertyChanged("AddNewCommand");

        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
