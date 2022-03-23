using HFWeatherApp_WPF.Model;
using HFWeatherApp_WPF.ViewModel.Commands;
using HFWeatherApp_WPF.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HFWeatherApp_WPF.ViewModel
{
    public class WeatherVM : INotifyPropertyChanged       
    {

        //Constructor
        public WeatherVM()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                SelectedCity = new City
                {
                    LocalizedName = "New York"
                };

                CurrentConditions = new CurrentConditions
                {
                    WeatherText = "Partly cloudy",
                    Temperature = new Temperature
                    {
                        Metric = new Units()
                        {
                            Value = "21"
                        }
                    }

                };

            }

            SearchCommand = new SearchCommand(this);

            //Observable list have to be initialized only once otherwise any
            //binding will be lost.
            Cities = new ObservableCollection<City>();

        }

        //Declare the properties 
        private string query;

        public string Query
        {
            get { return query; }
            set 
            { 
                query = value;

                //In the setter we need to trigger the event PropertyChanged, we can trigger it by
                //calling the OnPropertyChanged Method & passing the name of the peoperty that is changed 
                OnPropertyChanged("Query"); 

                //This event now is going to update anyone who is subscriping to 
                //letting them know that this property has changed.
                //This could  be possible because in the xaml file the TextBlocks for these proprties
                //Can subscripe to that property so that whenever the value of the property
                //changes, the text inside these textblocks changes as well.

                //Also when the text inside these textbox changes, that is going to be  updateing 
                //the Query that is can be used to call the API Query property
            }
        }

        // Properties 

        private CurrentConditions currentConditions;

        public CurrentConditions CurrentConditions
        {
            get { return currentConditions; }
            set 
            {
                currentConditions = value;
                OnPropertyChanged("CurrentConditions");
            }
        }

        private City selectedCity;

        public City SelectedCity
        {
            get { return selectedCity; }
            set 
            { 
                selectedCity = value;
                OnPropertyChanged("SelectedCity");
                GetCurrentConditions();
            }
        }


        public ObservableCollection<City> Cities { get; set; }
        public SearchCommand SearchCommand { get; set; }


        private async void GetCurrentConditions()
        {
            //Clear the query as the user already choose the city
            Query = string.Empty;

            //Clear the cities
            Cities.Clear();

             CurrentConditions = await AccuWeatherHelper.GetCurrentConditions(SelectedCity.Key);

        }
        public async void makeQuery()
        {
            var cities = await AccuWeatherHelper.GetCities(Query);

            //before adding the cities to the observable list
            //make sure to clear anyvalue inside this collection
            Cities.Clear();
            
            foreach(var city in cities)
            {
                Cities.Add(city);
            }

           
        }

        //Trigger this event when these properties change
        public event PropertyChangedEventHandler PropertyChanged;


        //Create a method to trigget the event Property changed
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
