using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Models;
using WeatherApp.Services;
using WeatherApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeatherApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainTabbedPage : TabbedPage
    {
        public MainTabbedPage ()
        {
            InitializeComponent();
            this.Title = "Weather App";

            this.ItemsSource = new CityWeatherViewModel[]
            {
               new CityWeatherViewModel(new NamedCity("Colombo",79.861243,6.9270786), new RestServices()), 
               new CityWeatherViewModel(new NamedCity("Cologne",6.960278,50.937531), new RestServices())
            };

            this.ItemTemplate = new DataTemplate(() => { return new CityWeatherPage(); });
        }

        async void ClickedAsync(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CityEntryListPage(new CityEntryListViewModel()));
        }
    }
}