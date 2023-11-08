using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Hangman.Model;
using Hangman.Commands;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace Hangman.ViewModel
{
    class GameViewModel : ViewModelBase
    {
        Users user;
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        public int delay { get; set; }
        private int delay1;
        public int Delay { get { return delay1; } set { delay1 = value; SetPropertyChanged(nameof(Delay)); } }
        public DateTime deadline { get; set; }
        private string currentImage;
        public string CurrentImage {
            get
            {
                return currentImage;
            }
            set
            {
                if (currentImage == value) return;
                currentImage = value;
                SetPropertyChanged("CurrentImage");
            }
        }
        private string visibility="Hidden";
        public string Visibility { get
            {
                return visibility;
            } set
            {
                visibility = value;
                SetPropertyChanged(nameof(Visibility));
            }
        }
        public GameViewModel()
        {
            delay = 30;
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            letters = new ObservableCollection<MyLetter>();
            word = new ObservableCollection<MyLetter>();
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            int secondsRemaining = (deadline - DateTime.Now).Seconds;
            if (secondsRemaining == 0)
            {
                dispatcherTimer.Stop();
                dispatcherTimer.IsEnabled = false;
                MessageBox.Show("Time has expired! You lose!");
                GameFinished();
                delay = 30;
            }
            else
            {
                Delay = secondsRemaining;
            }
        }
        private ObservableCollection<MyLetter> word;
        private int lives;
        private const int initialLives = 7;
        public int Lives { get { return lives; } set { lives = value; SetPropertyChanged(nameof(Lives)); } }
        public ObservableCollection<MyLetter> Word { get
            {
                return word;
            }
            set
            {
                if (word == value) return;
                word = value;
                NotifyPropertyChanged(nameof(Word));
            }
        }
        private ObservableCollection<MyLetter> letters;
        public ObservableCollection<MyLetter> Letters {
            get
            {
                return letters;
            }
            set
            {

            }
        }
        private ICommand newGame;
        public ICommand NewGame
        {
            get
            {
                if (newGame == null)
                    newGame = new RelayCommand(NewGameCommand);
                return newGame;

            }
        }
        private void StartTimer()
        {
            deadline = DateTime.Now.AddSeconds(delay);
            dispatcherTimer.Start();
        }

        private void NewGameCommand(object parameter)
        {
            StartTimer();
            CurrentImage = AppDomain.CurrentDomain.BaseDirectory+"Images\\1.png";
            Delay = 30;
            Lives = 7;
            Visibility = "Visible";
            Letters.Clear();
            Word.Clear();
            for (int i = 0; i < 26; i++)
            {
                int number = 65 + i;
                char letter = (char)number;
                bool b = true;
                string s = "";
                s += letter;
                MyLetter l = new MyLetter(s, b, "Hidden");
                Letters.Add(l);
            }
                string[] words = new string[] {
                "WORD", "TEST", "LANGUAGE", "DEFAULT",
                "WORLD", "SHIP", "FLEET", "BLUEPRINT", "WASHINGTON", "TANK", "CAR", "STING", "TIGER", "EUROPE", "PAIN", "OCEAN"};
            string w = words[new Random().Next(0, words.Length)];
            for (int i = 0; i < w.Length; i++)
            {
                string s = "";
                s += w[i];
                MyLetter l = new MyLetter(s, false, "Hidden");
                Word.Add(l);
            }
            NotifyPropertyChanged("NewGame");
        }
        private ICommand letterPressed;
        public ICommand LetterPressed
        {
            get
            {
                if (letterPressed == null)
                    letterPressed = new RelayCommand(LettPressed);
                return letterPressed;
            }
        }
        void LettPressed(object parameter)
        {

            if(parameter is MyLetter l)
            {
                int ok = 0;
                for (int i = 0; i < Word.Count; i++)
                {
                    if (Word[i].Letter == l.Letter)
                    {
                        Word[i].Visible = "Visible";
                        ok = 1;
                    }
                }
                if (ok==0)
                {
                    Lives--;
                    int currentLives = initialLives - Lives + 1;
                    CurrentImage = AppDomain.CurrentDomain.BaseDirectory+"\\Images\\"+currentLives.ToString()+".png";
                    if (Lives==0)
                    {
                        GameFinished();
                        MessageBox.Show("You lose!");
                    }
                    
                }
                int ok1 = 1;
                for (int i = 0; i < Word.Count; i++)
                {
                    if (Word[i].Visible == "Hidden")
                        ok1 = 0;
                }
                if (ok1 == 1)
                {
                    GameFinished();
                    MessageBox.Show("You win!");
                    
                }
                for (int j = 0; j < Letters.Count; j++)
                    if (Letters[j].Letter == l.Letter)
                        Letters[j].Enable = false;

            }
            SetPropertyChanged("LetterPressed");
        }
        private void GameFinished()
        {
            dispatcherTimer.Stop();
            for (int j = 0; j < Letters.Count; j++)
                    Letters[j].Enable = false;
        }
        private void Letters_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SetPropertyChanged("Letters");
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
