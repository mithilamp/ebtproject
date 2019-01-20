using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using WeatherApp.Models;
using WeatherApp.Services;
using WeatherApp.Views;
using Xamarin.Forms;

namespace WeatherApp.ViewModels
{
    public class MainTabbedViewmodel :BaseViewModel
    {
        RestServices RestService { get; set; }
        public ObservableCollection<CityWeatherViewModel> ViewModelsList { get; set; }

        public MainTabbedViewmodel()
        {
            this.RestService = new RestServices();
            var list = new List<CityWeatherViewModel>
            {
               new CityWeatherViewModel(new NamedCity(79.861243,6.9270786,"Colombo"), RestService),
               new CityWeatherViewModel(new NamedCity(6.960278,50.937531,"Cologne"), RestService)
            };
            ViewModelsList = new ObservableCollection<CityWeatherViewModel>();

            MessagingCenter.Subscribe<CityEntryListViewModel, NamedCity>(this, "delete", (sender, obj) =>
            {
                RemovePage(obj);
            });

            MessagingCenter.Subscribe<AddCityPage, NamedCity>(this, "add", (sender, obj) =>
            {
                AddPage(obj);
            });
        }

        /// <summary>
        /// Adds the page.
        /// </summary>
        /// <param name="obj">The object.</param>
        private void AddPage(NamedCity obj)
        {
            ViewModelsList.Add(new CityWeatherViewModel(obj, RestService));
        }

        /// <summary>
        /// Removes the page.
        /// </summary>
        /// <param name="obj">The object.</param>
        private void RemovePage(NamedCity obj)
        {
            var list = new List<CityWeatherViewModel>(ViewModelsList);
            foreach (CityWeatherViewModel item in list)
            {
                if (item.NamedCity.Equals(obj))
                {
                    ViewModelsList.Remove(item);
                }
            }
        }
    }
}
