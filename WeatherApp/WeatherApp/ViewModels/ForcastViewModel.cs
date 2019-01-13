using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using WeatherApp.Interfaces;
using WeatherApp.Models;
using WeatherApp.Services;
using Xamarin.Forms;

namespace WeatherApp.ViewModels
{
    public class ForcastViewModel : BaseViewModel
    {
        /// <summary>
        /// The search command
        /// </summary>
        private Command searchCmd;

        /// <summary>
        /// The rest service
        /// </summary>
        public IRestServices restService;

        /// <summary>
        /// The weather data
        /// </summary>
        WeatherData weatherData;

        /// <summary>
        /// The forcast list
        /// </summary>
        IList<List> forcastList;

        public Task Init { get; private set; }

        /// <summary>
        /// Gets or sets the weather data.
        /// </summary>
        public WeatherData WeatherData {
            get { return weatherData; }
            set {
                if (weatherData != value)
                {
                    weatherData = value;
                    OnPropertyChanged("WeatherData");
                }
            }
        }

        /// <summary>
        /// Gets or sets the forcast list.
        /// </summary>
        public IList<List> ForcastList
        {
            get { return forcastList; }
            set
            {
                if (forcastList != value)
                {
                    forcastList = value;
                    forcastList.RemoveAt(0); //Exempt today
                    OnPropertyChanged("ForcastList");
                }
            }
        }

        /// <summary>
        /// Gets or sets the city entry.
        /// </summary>
        public string CityEntry { get; set; }

        /// <summary>
        /// Gets the search command.
        /// </summary>
        public Command SearchCmd { get => this.searchCmd ?? (this.searchCmd = new Command(async () => await this.SearchActionAsync())); }

        /// <summary>
        /// Starts the timer.
        /// </summary>
        public void SubscribeToUpdates()
        {
            Device.StartTimer(new TimeSpan(0, 3, 0, 0, 0), UpdateData); // Default: Request weather update every 3 hrs.
        }

        /// <summary>
        /// Searches the action asynchronous.
        /// </summary>
        private async Task SearchActionAsync()
        {
            if (!string.IsNullOrWhiteSpace(CityEntry))
            {
                restService.ChangeCity(CityEntry);
                await FetchDataAsync();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ForcastViewModel"/> class.
        /// </summary>
        /// <param name="services">The services.</param>
        public ForcastViewModel(RestServices services)
        {
            restService = services;
            WeatherData = new WeatherData();
            this.restService.ChangeCity("Koeln"); //Default search
            Init = FetchDataAsync();
        }

        /// <summary>
        /// Fetch latest weather update
        /// </summary>
        public async Task FetchDataAsync()
        {
            try
            {
                this.WeatherData = await restService.GetWeatherData();
                var timeOfDay = WeatherData.List[0].DtTxt.TimeOfDay; //last weather update
                this.ForcastList = this.WeatherData.List.Where(x => x.DtTxt.TimeOfDay == timeOfDay).ToList();
            }
            catch(Exception ex)
            {
                await Application.Current.MainPage?.DisplayAlert("Error", "Invalid City Entry", "Ok");
            }
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
