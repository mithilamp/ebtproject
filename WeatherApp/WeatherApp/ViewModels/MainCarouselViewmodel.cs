using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Interfaces;
using WeatherApp.Models;
using WeatherApp.Services;
using WeatherApp.Views;
using Xamarin.Forms;

namespace WeatherApp.ViewModels
{
    public class MainCarouselViewmodel :BaseViewModel
    {
        private readonly ICitiesRepository citiesRepository;
        public ObservableCollection<CityWeatherViewModel> ViewModelsList { get; set; }

        public MainCarouselViewmodel(ICitiesRepository citiesRepository)
        {
            this.citiesRepository = citiesRepository;
            //var list = new List<CityWeatherViewModel>
            //{
            //   new CityWeatherViewModel(new NamedCity(79.861243,6.9270786,"Colombo"), RestService),
            //   new CityWeatherViewModel(new NamedCity(6.960278,50.937531,"Cologne"), RestService)
            //};
            ViewModelsList = new ObservableCollection<CityWeatherViewModel>(GetPersistedData());

            MessagingCenter.Subscribe<CityEntryListViewModel, NamedCity>(this, "delete", async (sender, obj) =>
            {
                await RemovePage(obj);
            });

            MessagingCenter.Subscribe<AddCityPage, NamedCity>(this, "add", async (sender, obj) =>
            {
                await AddPage(obj);
            });
        }

        /// <summary>
        /// Gets the persisted data.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<CityWeatherViewModel> GetPersistedData()
        {
            var cities = citiesRepository.GetCitiesAsync().Result;
            var items = new List<CityWeatherViewModel>();
            foreach (var city in cities)
            {
                items.Add(new CityWeatherViewModel(city));
            }
            return items;
        }

        /// <summary>
        /// Updates the device location.
        /// </summary>
        /// <param name="namedCity">The named city.</param>
        public async Task UpdateDeviceLocation(NamedCity namedCity)
        {
            if(ViewModelsList.Count != 0)
            {
                foreach (CityWeatherViewModel item in ViewModelsList)
                {
                    if (item.NamedCity.Name.Equals(namedCity.Name))
                    {
                        item.NamedCity.Latitude = namedCity.Latitude;
                        item.NamedCity.Longitude = namedCity.Longitude;
                        await citiesRepository.UpdateCityAsync(item.NamedCity);
                    }
                }
            }
            else
            {
                ViewModelsList.Add(new CityWeatherViewModel(namedCity));
                await citiesRepository.AddCityAsync(namedCity);
            }
        }

        /// <summary>
        /// Adds the page.
        /// </summary>
        /// <param name="city">The object.</param>
        private async Task AddPage(NamedCity city)
        {
            ViewModelsList.Add(new CityWeatherViewModel(city));
            await citiesRepository.AddCityAsync(city);
        }

        /// <summary>
        /// Removes the page.
        /// </summary>
        /// <param name="obj">The object.</param>
        private async Task RemovePage(NamedCity obj)
        {
            var list = new List<CityWeatherViewModel>(ViewModelsList);
            foreach (CityWeatherViewModel item in list)
            {
                if (item.NamedCity.Equals(obj))
                {
                    ViewModelsList.Remove(item);
                    await citiesRepository.RemoveCityAsync(obj);
                }
            }
        }
    }
}
