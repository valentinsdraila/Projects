using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangman.Model
{
    class MyLetter : INotifyPropertyChanged
    {
        private string letter;
        public string Letter
        { get
            {
                return letter;
            }
            set
            {
                if (letter == value) return;
                letter = value;
                NotifyPropertyChanged("Letter");
            }
        }
        private bool enable;
        public bool Enable { get
            {
                return enable;
            }

            set
            {
                if (enable == value) return;
                enable = value;
                NotifyPropertyChanged("Enable");
            }
        }
        private string visible;
        public string Visible
        {
            get
            {
                return visible;
            }
            set
            {
                if (visible == value) return;
                visible = value;
                NotifyPropertyChanged("Visible");
            }
        }
        public MyLetter(string Lett, bool Enbl, string Visib)
        {
            Letter = Lett;
            Enable = Enbl;
            Visible = Visib;
            NotifyPropertyChanged("Letter");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
