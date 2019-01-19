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
	public partial class CityEntryListPage : ContentPage
	{
		public CityEntryListPage (CityEntryListViewModel viewModel)
		{
            this.BindingContext = viewModel;
			InitializeComponent ();
		}

        async void ClickedAsync(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddCityPage());
        }
    }
}