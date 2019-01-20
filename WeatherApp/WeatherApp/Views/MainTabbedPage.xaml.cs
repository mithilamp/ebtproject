using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Models;
using WeatherApp.Services;
using WeatherApp.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeatherApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainTabbedPage : TabbedPage
    {
        RestServices RestService { get; set; }
        public MainTabbedPage ()
        {
            InitializeComponent();
            this.Title = "Weather App";
            this.RestService = new RestServices();

            //this.ItemsSource = new CityWeatherViewModel(GetCurrentLocation(),RestService);
            this.ItemsSource = new CityWeatherViewModel[]
            {
               new CityWeatherViewModel(new NamedCity(79.861243,6.9270786,"Colombo"), RestService),
               new CityWeatherViewModel(new NamedCity(6.960278,50.937531,"Cologne"), RestService)
            };

            this.ItemTemplate = new DataTemplate(() => { return new CityWeatherPage(); });

            MessagingCenter.Subscribe<CityEntryListViewModel, NamedCity>(this, "delete", (sender, obj) =>
            {
                RemovePage(obj);
            });

            MessagingCenter.Subscribe<AddCityPage, NamedCity>(this, "add", (sender, obj) =>
            {
                AddPage(obj);
            });
        }

        private async Task<NamedCity> GetCurrentLocation()
        {
            NamedCity namedCity = null;
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                var location = await Geolocation.GetLocationAsync(request);

                namedCity = new NamedCity(location.Longitude, location.Latitude);
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
            return namedCity;
        }

        /// <summary>
        /// Adds the page.
        /// </summary>
        /// <param name="obj">The object.</param>
        private void AddPage(NamedCity obj)
        {
            var items = new List<CityWeatherViewModel>();
            foreach (var page in this.Children)
            {
                var viewModel = page.BindingContext as CityWeatherViewModel;
                items.Add(viewModel);                    
            }
            items.Add(new CityWeatherViewModel(obj, RestService));
            this.ItemsSource = items;
        }

        /// <summary>
        /// Removes the page.
        /// </summary>
        /// <param name="obj">The object.</param>
        private void RemovePage(NamedCity obj)
        {
            var items = new List<CityWeatherViewModel>();
            foreach (var page in this.Children)
            {
                var viewModel = page.BindingContext as CityWeatherViewModel;
                if (!viewModel.NamedCity.Equals(obj))
                {
                    items.Add(viewModel);
                }
            }
            this.ItemsSource = items;
        }

        /// <summary>
        /// Clickeds the asynchronous.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        async void ClickedAsync(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CityEntryListPage(new CityEntryListViewModel()));
        }
    }
}