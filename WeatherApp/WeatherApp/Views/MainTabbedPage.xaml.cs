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

            MessagingCenter.Subscribe<CityEntryListViewModel, NamedCity>(this, "delete", (sender, obj) =>
            {
                RemovePage(obj);
            });
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
                if (!viewModel.namedCity.Equals(obj))
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