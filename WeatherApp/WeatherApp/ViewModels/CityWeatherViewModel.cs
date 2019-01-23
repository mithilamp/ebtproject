using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Interfaces;
using WeatherApp.Models;
using WeatherApp.Services;
using Xamarin.Forms;

namespace WeatherApp.ViewModels
{
    public class CityWeatherViewModel :BaseViewModel
    {
        private NamedCity namedCity;

        /// <summary>
        /// Gets or sets the named city.
        /// </summary>
        public NamedCity NamedCity
        {
            get { return namedCity; }
            set
            {
                if (namedCity != value)
                {
                    namedCity = value;
                    OnPropertyChanged("NamedCity");
                }
            }
        }

        /// <summary>
        /// The display weather
        /// </summary>
        private WeatherPropertiesViewModel displayWeather;

        /// <summary>
        /// The rest service
        /// </summary>
        private IRestServices restService;

        /// <summary>
        /// Gets the initialize.
        /// </summary>
        public Task Init { get; private set; }

        /// <summary>
        /// Switches to top command.
        /// </summary>
        /// <param name="forcast">The forcast.</param>
        public void SwitchToTopCmd(List forcast)
        {
            DisplayWeather.PopulateHeaderData(forcast);
        }


        /// <summary>
        /// Gets or sets the display weather.
        /// </summary>
        public WeatherPropertiesViewModel DisplayWeather
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
        public CityWeatherViewModel(NamedCity namedCity)
        {
            this.NamedCity = namedCity;
            this.DisplayWeather = new WeatherPropertiesViewModel();
            restService = new RestServices();
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
                var weatherData = await restService.GetWeatherData(this.NamedCity);
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
            Device.StartTimer(new TimeSpan(0, 3, 0, 0, 0), UpdateData); // Default: Request weather update every 3 hrs.
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
