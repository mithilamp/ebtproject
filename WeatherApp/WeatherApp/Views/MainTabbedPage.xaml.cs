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

        private async Task GetCurrentLocation()
        {
            var viewModel = this.BindingContext as MainTabbedViewmodel;
            try
            {
                //var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                //var location = await Geolocation.GetLocationAsync(request);
                var namedCity = new NamedCity();
                var location = await Geolocation.GetLastKnownLocationAsync();
                namedCity.Name = "CurrentLocation";
                namedCity.Longitude = location.Longitude;
                namedCity.Latitude = location.Latitude;
                await viewModel.UpdateDeviceLocation(namedCity);

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

        /// <summary>
        /// When overridden, allows application developers to customize behavior immediately prior to the <see cref="T:Xamarin.Forms.Page" /> becoming visible.
        /// </summary>
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await GetCurrentLocation();
        }
    }
}