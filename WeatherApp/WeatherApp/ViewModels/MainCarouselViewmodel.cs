using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        /// <summary>
        /// The cities repository
        /// </summary>
        private readonly ICitiesRepository citiesRepository;

        /// <summary>
        /// The google map services
        /// </summary>
        private IGoogleMapServices googleMapServices;

        /// <summary>
        /// Gets or sets the view models list.
        /// </summary>
        public ObservableCollection<CityWeatherViewModel> ViewModelsList { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainCarouselViewmodel"/> class.
        /// </summary>
        /// <param name="citiesRepository">The cities repository.</param>
        public MainCarouselViewmodel(ICitiesRepository citiesRepository)
        {
            this.citiesRepository = citiesRepository;
            googleMapServices = new GoogleMapServices();
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
        /// Updates the device current location.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="Longitude">The longitude.</param>
        /// <returns></returns>
        public async Task UpdateDeviceLocation(NamedCity namedCity)
        {
            //var namedCity = await googleMapServices.GetReverseGeocodingPlace(latitude, Longitude);
            if (ViewModelsList.Count != 0)
            {
                ViewModelsList[0].NamedCity = namedCity;
                await UpdateRepo(namedCity);           
            }
            else
            {
                ViewModelsList.Add(new CityWeatherViewModel(namedCity));
                await citiesRepository.AddCityAsync(namedCity);
            }
        }

        /// <summary>
        /// Updates the repository
        /// </summary>
        /// <param name="namedCity">The named city.</param>
        /// <returns></returns>
        private async Task UpdateRepo(NamedCity namedCity)
        {
            var currentCity = citiesRepository.GetCitiesAsync().Result;
            currentCity.FirstOrDefault().Name = namedCity.Name;
            currentCity.FirstOrDefault().Latitude = namedCity.Latitude;
            currentCity.FirstOrDefault().Longitude = namedCity.Longitude;
            await citiesRepository.UpdateCityAsync(currentCity.FirstOrDefault());
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
