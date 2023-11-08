using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangman.ViewModel
{
        public abstract class ViewModelBase : INotifyPropertyChanged
        {
            #region Property Changed Event Handler
            public void SetPropertyChanged(string propertyName)
            {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
            public event PropertyChangedEventHandler PropertyChanged;
            #endregion Property Changed Event Handler
        }
}
