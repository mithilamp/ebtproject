using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Models;
using WeatherApp.Views;
using Xamarin.Forms;

namespace WeatherApp.ViewModels
{
    public class CityEntryListViewModel :BaseViewModel
    {
        /// <summary>
        /// Gets or sets the named city list.
        /// </summary>
        public ObservableCollection<NamedCity> NamedCityList { get; set; }

        /// <summary>
        /// The remove command
        /// </summary>
        private Command removeCmd;

        /// <summary>
        /// Gets the remove command.
        /// </summary>
        public Command RemoveCmd { get => this.removeCmd ?? (this.removeCmd = new Command( (e) =>  this.RemoveItemAction(e as NamedCity))); }

        /// <summary>
        /// Removes the item action.
        /// </summary>
        /// <param name="namedCity">The named city.</param>
        private void RemoveItemAction(NamedCity namedCity)
        {
            if (this.NamedCityList.Count > 1)
            {
                this.NamedCityList.Remove(namedCity);
                MessagingCenter.Send<CityEntryListViewModel, NamedCity>(this, "delete", namedCity);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CityEntryListViewModel"/> class.
        /// </summary>
        public CityEntryListViewModel()
        {
            NamedCityList = new ObservableCollection<NamedCity>(GetAvailableCityEntries());

            MessagingCenter.Subscribe<AddCityPage, NamedCity>(this, "add", (sender, obj) =>
            {
                NamedCityList.Add(obj);
            });
        }

        /// <summary>
        /// Gets the available city entries.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<NamedCity> GetAvailableCityEntries()
        {
            var items = new List<NamedCity>();
            var mainPage = Application.Current.MainPage;
            if (mainPage is NavigationPage)
            {
                var navigationPage = (NavigationPage)mainPage;
                if (navigationPage.CurrentPage is CarouselPage)
                {
                    var tabbedPage = (CarouselPage)navigationPage.CurrentPage;
                    foreach (var page in tabbedPage.Children)
                    {
                        var viewModel = page.BindingContext as CityWeatherViewModel;
                        items.Add(viewModel.NamedCity);
                    }
                }
            }
            return items;
        }
    }
}
