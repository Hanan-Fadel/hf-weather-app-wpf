using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HFWeatherApp_WPF.ViewModel.Commands
{
    public class SearchCommand : ICommand
    {
        public WeatherVM VM { get; set; }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value;  }
            remove { CommandManager.RequerySuggested -= value; }
        }


        public SearchCommand(WeatherVM vm)
        {
            VM = vm;
        }
       
        public bool CanExecute(object parameter)
        {
             string query = parameter as string;

            if (string.IsNullOrWhiteSpace(query))
                return false;
            return true;

        }

        //This method will be executed if the canExecute returned true
        public void Execute(object parameter)
        {
            VM.makeQuery();
        }
    }
}
