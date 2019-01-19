using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Interfaces;
using WeatherApp.Models;
using Xamarin.Forms;

namespace WeatherApp.ViewModels
{
    public class CityWeatherViewModel :BaseViewModel
    {
        public NamedCity namedCity { get; set; }

        public WeatherOnTopViewModel displayWeather;

        /// <summary>
        /// The rest service
        /// </summary>
        public IRestServices restService;

        /// <summary>
        /// Gets the initialize.
        /// </summary>
        public Task Init { get; private set; }

        /// <summary>
        /// Gets or sets the display weather.
        /// </summary>
        public WeatherOnTopViewModel DisplayWeather
        {
            get { return displayWeather; }
            set
            {
                if (displayWeather != value)
                {
                    displayWeather = value;
                    OnPropertyChanged("DisplayWeather");
                }
            }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="CityWeatherViewModel"/> class.
        /// </summary>
        /// <param name="namedCity">The named city.</param>
        public CityWeatherViewModel(NamedCity namedCity, IRestServices services)
        {
            this.namedCity = namedCity;
            this.DisplayWeather = new WeatherOnTopViewModel();
            restService = services;
            Init = FetchDataAsync();
        }

        /// <summary>
        /// Fetches the data asynchronous.
        /// </summary>
        /// <returns></returns>
        private async Task FetchDataAsync()
        {
            try
            {
                var weatherData = await restService.GetWeatherData(this.namedCity);
                this.DisplayWeather.GenerateNewData(weatherData);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage?.DisplayAlert("Error", "Invalid City Entry", "Ok");
            }
        }

        /// <summary>
        /// Subscribes to updates.
        /// </summary>
        internal void SubscribeToUpdates()
        {
            Device.StartTimer(new TimeSpan(0, 0, 0, 3, 0), UpdateData); // Default: Request weather update every 3 hrs.
        }

        /// <summary>
        /// Updates the weather data.
        /// </summary>
        private bool UpdateData()
        {
            Init = FetchDataAsync();
            return true;
        }
    }
}
