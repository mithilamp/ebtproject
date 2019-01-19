using System;
using System.Collections.Generic;
using System.Text;
using WeatherApp.Models;

namespace WeatherApp.ViewModels
{
    public class CityWeatherViewModel
    {
        public string CityName { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }

        public CityWeatherViewModel(NamedCity namedCity)
        {
            CityName = namedCity.Name;
            lat = namedCity.Latitude;
            lon = namedCity.Longitude;
        }
    }
}
