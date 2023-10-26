using System;
using System.Windows.Input;

namespace SherlockUI.Assets
{
  class RelayCommand : ICommand

  {

    private readonly Action<object> commandTask;




    public RelayCommand(Action<object> workToDo)

    {

      commandTask = workToDo;

    }

    public event EventHandler CanExecuteChanged
    {
      add
      {
        CommandManager.RequerySuggested += value;
      }
      remove
      {
        CommandManager.RequerySuggested -= value;
      }
    }
    public bool CanExecute(object parameter)

    {
      return true;

    }

    public void Execute(object parameter)
    {
      commandTask(parameter);
    }
  }
}
