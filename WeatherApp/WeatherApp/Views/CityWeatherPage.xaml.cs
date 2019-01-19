using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeatherApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CityWeatherPage : ContentPage
	{
		public CityWeatherPage()
		{
			InitializeComponent ();
            this.SetBinding(ContentPage.TitleProperty, "DisplayWeather.CityName");
        }

        /// <summary>
        /// When overridden, allows application developers to customize behavior immediately prior to the <see cref="T:Xamarin.Forms.Page" /> becoming visible.
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            var viewModel = this.BindingContext as CityWeatherViewModel;
            viewModel.SubscribeToUpdates();
        }
    }
}