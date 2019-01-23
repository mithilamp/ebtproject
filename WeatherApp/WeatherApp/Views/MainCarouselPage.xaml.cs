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
    public partial class MainCarouselPage : CarouselPage
    {
        public MainCarouselPage (MainCarouselViewmodel viewmodel)
        {
            InitializeComponent();
            this.BindingContext = viewmodel;
            this.Title = "Weather App";

            this.SetBinding(CarouselPage.ItemsSourceProperty, "ViewModelsList", BindingMode.TwoWay);

            this.ItemTemplate = new DataTemplate(() => { return new CityWeatherPage(); });
        }

        /// <summary>
        /// Gets the current location.
        /// </summary>
        /// <returns></returns>
        private async Task GetCurrentLocation()
        {
            var viewModel = this.BindingContext as MainCarouselViewmodel;
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                var location = await Geolocation.GetLocationAsync(request);
                //var location = await Geolocation.GetLastKnownLocationAsync();

                var placemarks = await Geocoding.GetPlacemarksAsync(location.Latitude,location.Longitude);
                var placemark = placemarks.ToList()[2];
                if (placemark != null)
                {
                    await viewModel.UpdateDeviceLocation(GetNamedCity(placemark, location));
                }
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
        /// Gets the named city.
        /// </summary>
        /// <param name="placemark">The placemark.</param>
        /// <param name="location">The location.</param>
        /// <returns></returns>
        private NamedCity GetNamedCity(Placemark placemark, Location location)
        {
            var namedCity = new NamedCity();
            namedCity.Name = placemark.FeatureName;
            namedCity.Longitude = location.Longitude;
            namedCity.Latitude = location.Latitude;
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