using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml;
using SherlockUI.Assets;
using SherlockUI.Models;
using SherlockUI.Views;
using SherlockUtil;

namespace SherlockUI.ViewModels
{
  public class MainWindowViewModel : ViewModelBase
  {

    private readonly string iniPath = @"..\..\..\Config\Sherlock.ini";
    private ReadWriteINIfile iniUtil { get; set; }
    private bool enableTextExtraction;
    public bool EnableTextExtraction
    {
      get
      {
        return enableTextExtraction;
      }
      set
      {
        enableTextExtraction = value;
        OnPropertyChanged(nameof(EnableTextExtraction));
        if(EnableTextExtraction)
        {
          iniUtil.WriteINI("GlobalSettings", "EnableTextExtraction", "1");
        }
        else
        {
          iniUtil.WriteINI("GlobalSettings", "EnableTextExtraction", "0");
        }
      }
    }
    private string status;
    public string Status
    {
      get
      {
        return status;
      }
      set
      {
        status = value;
        OnPropertyChanged(nameof(Status));
      }
    }
    private string progressString;
    public string ProgressString
    {
      get
      {
        return progressString;
      }
      set
      {
        progressString = value;
        OnPropertyChanged(nameof(ProgressString));
      }
    }
    private float progress;
    public float Progress
    {
      get
      {
        return progress;
      }
      set
      {
        progress = value;
        ProgressString = progress.ToString("0.0") + "%";
        OnPropertyChanged(nameof(Progress));
      }
    }
    private ObservableCollection<LogMessage> messagesList;
    public ObservableCollection<LogMessage> MessagesList
    {
      get
      {
        return messagesList;
      }
      set
      {
        messagesList = value;
        OnPropertyChanged(nameof(MessagesList));
      }
    }

    public MainWindowViewModel()
    {
      iniUtil = new ReadWriteINIfile(iniPath);
      MessagesList = new ObservableCollection<LogMessage>();
      EnableTextExtraction = false;
      try
      {
        int enableValue = int.Parse(iniUtil.ReadINI("GlobalSettings", "EnableTextExtraction")[0]);
        EnableTextExtraction = enableValue != 0;
      }
      catch (Exception e)
      {
        MessagesList.Add(new LogMessage(e.Message, new BitmapImage(new Uri(@"/green.png", UriKind.Relative))));
      }
    }
    private ICommand generateCommand;
    public ICommand GenerateCommand
    {
      get
      {
        if (generateCommand == null)
          generateCommand = new RelayCommand(Generate);
        return generateCommand;
      }
    }
    public delegate void ProgressCallback(float progress);
    public delegate void TextBoxCallback(string text, bool status);
    public void Generate(object parameter)
    {
      progress = 0;
      Task.Run(() =>
      {
        var message = new WindowMessage((text, msgstatus)=>
        {
          if (msgstatus)
            Application.Current.Dispatcher.Invoke(() => MessagesList.Add(new LogMessage(text, new BitmapImage(new Uri(@"/green.png", UriKind.Relative)))));
          else
            Application.Current.Dispatcher.Invoke(() => MessagesList.Add(new LogMessage(text, new BitmapImage(new Uri(@"/red.png", UriKind.Relative)))));
          Application.Current.Dispatcher.Invoke(() => Status = text);
        });
        message.Write("Starting analysis.", true);
        ReadWriteINIfile iniUtil = new ReadWriteINIfile(iniPath);
        string[] args = new string[0];
        Task.Run(() => UpdateProgress());
        XmlDocument failedTestsXML = FailedTests.Retrieve(iniUtil, message, args);
        if (failedTestsXML.HasChildNodes == false || failedTestsXML == null)
        {
          message.Write("Error while retrieving failed tests data", false);
          return;
        }
        ATReport atReport = new ATReport(failedTestsXML);
        atReport.ProgressCallback = (progress) =>
        {
          Progress = progress;
        };
        if (atReport.Create(iniPath, message))
        {
          atReport.CompareToLastReport(message);
          atReport.SaveReport(message, true);
        }
        GC.Collect();
      });
    }

    private ICommand rescheduleCommand;
    public ICommand RescheduleCommand
    {
      get
      {
        if (rescheduleCommand == null)
          rescheduleCommand = new RelayCommand(Reschedule);
        return rescheduleCommand;
      }
    }
    public void Reschedule(object parameter)
    {
      Status = "Rescheduling tests...";
      Task.Run(() =>
      {
        var message = new WindowMessage((text, msgstatus) =>
        {
          if (msgstatus)
            Application.Current.Dispatcher.Invoke(() => MessagesList.Add(new LogMessage(text, new BitmapImage(new Uri(@"/green.png", UriKind.Relative)))));
          else
            Application.Current.Dispatcher.Invoke(() => MessagesList.Add(new LogMessage(text, new BitmapImage(new Uri(@"/red.png", UriKind.Relative)))));
          Application.Current.Dispatcher.Invoke(() => Status = text);
        });
        ReadWriteINIfile iniUtil = new ReadWriteINIfile(iniPath);
        string watsonOutputPath = iniUtil.ReadINI("GlobalSettings", "WatsonOutputPath")[0];
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(watsonOutputPath);
        ATReport rescheduleATReport = new ATReport(xmlDocument);
        bool status = rescheduleATReport.RescheduleTests(message);
        GC.Collect();
        if (status)
        {
          Status = "Rescheduling finished successfully!";
          Application.Current.Dispatcher.Invoke(() => MessagesList.Add(new LogMessage(Status, new BitmapImage(new Uri(@"/green.png", UriKind.Relative)))));
        }
        else
        {
          Status = "Rescheduling failed!";
          Application.Current.Dispatcher.Invoke(() => MessagesList.Add(new LogMessage(Status, new BitmapImage(new Uri(@"/red.png", UriKind.Relative)))));
        }
      });
    }

    private ICommand editCommand;
    public ICommand EditCommand
    {
      get
      {
        if (editCommand == null)
          editCommand = new RelayCommand(Edit);
        return editCommand;
      }
    }
    private void Edit(object parameter)
    {
      EditINIDialog iniDialog = new EditINIDialog();
      iniDialog.Show();
    }
  

    private string textBoxContent;
    public string TextBoxContent
    {
      get { return textBoxContent; }
      set
      {
        textBoxContent = value;
        OnPropertyChanged(nameof(TextBoxContent));
      }

    }

    private void UpdateProgress()
    {
      for(int i=0;i<50;i++)
      {
        Progress += 1;
        System.Threading.Thread.Sleep(100);
      }
    }
  }
}
