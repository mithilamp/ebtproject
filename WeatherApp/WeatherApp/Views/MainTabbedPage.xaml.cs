using Plugin.Geolocator;
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
        NamedCity CurrentCity { get; set; }
        public MainTabbedPage (MainTabbedViewmodel viewmodel)
        {
            InitializeComponent();
            this.BindingContext = viewmodel;
            this.Title = "Weather App";

            this.SetBinding(TabbedPage.ItemsSourceProperty, "ViewModelsList", BindingMode.TwoWay);

            this.ItemTemplate = new DataTemplate(() => { return new CityWeatherPage(); });
        }

        private async Task<NamedCity> GetCurrentLocation()
        {
            NamedCity namedCity = null;
            try
            {
                //var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                //var location = await Geolocation.GetLocationAsync(request);
                var location = await Geolocation.GetLastKnownLocationAsync();

                namedCity = new NamedCity(location.Longitude, location.Latitude);
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await DisplayAlert("Faild", fnsEx.Message, "OK");
            }
            catch (PermissionException pEx)
            {
                await DisplayAlert("Faild", pEx.Message, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Faild", ex.Message, "OK");
            }
            return namedCity;
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

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;

            var position = await locator.GetPositionAsync();
            //temp = new NamedCity(position.Longitude,position.Latitude);
            //var t = GetCurrentLocation().Result;
            //Task.Run(async ()=> await GetCurrentLocation());
        }
    }
}