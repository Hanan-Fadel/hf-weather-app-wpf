﻿using HFWeatherApp_WPF.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HFWeatherApp_WPF.ViewModel.Helpers
{
    public class AccuWeatherHelper
    {
      
        public const string BASE_URL = "http://dataservice.accuweather.com/";
        public const string AUTOCOMPLETE_ENDPOINT = "locations/v1/cities/autocomplete/?apikey={0}&q={1}";
        public const string CURRENT_CONDITIONS_ENDPOINT = "currentconditions/v1/{0}?apikey={1}";
        public const string API_KEY = "fhsUuCWggwlSdqVizWttAUllUewC0lQC";

        //A method to return list of cities

        //Note: If we called the menthod GetCities without using await, 
        //It will return to us the task itself
        public static async Task<List<City>> GetCities(string query)
        {
            List<City> cities = new List<City>();
            string url = BASE_URL + string.Format(AUTOCOMPLETE_ENDPOINT, API_KEY, query);
            using (HttpClient client=new HttpClient())
            {
                var response = await client.GetAsync(url);

                //read the json from the response 
                string json = await response.Content.ReadAsStringAsync();

                //Deserialize JSON
                cities = JsonConvert.DeserializeObject<List<City>>(json);



            }
                return cities;
        }


        //Receiving the Current conditions to selected city
        public static async Task<CurrentConditions> GetCurrentConditions(string cityKey)
        {
            CurrentConditions currentConditions = new CurrentConditions();

            string url = BASE_URL + string.Format(CURRENT_CONDITIONS_ENDPOINT, cityKey, API_KEY);
            using (HttpClient client= new HttpClient())
            {
                var response = await client.GetAsync(url);

                //read the json from the response 
                string json = await response.Content.ReadAsStringAsync();

                //Deserialize JSON
                currentConditions = (JsonConvert.DeserializeObject<List<CurrentConditions>>(json)).FirstOrDefault();

            }
            return currentConditions;
        }
    }
}