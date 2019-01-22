using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Models;
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
			InitializeComponent();
            this.SetBinding(ContentPage.TitleProperty, "NamedCity.Name");
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

        /// <summary>
        /// Called when [tapped].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void OnTapped(object sender, EventArgs e)
        {
            var viewModel = this.BindingContext as CityWeatherViewModel;
            var layout = sender as StackLayout;
            viewModel.SwitchToTopCmd(layout.BindingContext as List);
        }
    }
}