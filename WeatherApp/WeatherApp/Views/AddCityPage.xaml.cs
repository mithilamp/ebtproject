using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Interfaces;
using WeatherApp.Models;
using WeatherApp.Services;
using WeatherApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeatherApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddCityPage : ContentPage
	{
        public AddCityPage (AddCityViewModel addCityViewModel)
		{
            InitializeComponent();
            BindingContext = addCityViewModel;
            SearchBar.PlacesRetrieved += Search_Bar_PlacesRetrieved;
            SearchBar.TextChanged += Search_Bar_TextChanged;
            SearchBar.MinimumSearchText = 2;
            results_list.ItemSelected += Results_List_ItemSelected;
        }

        /// <summary>
        /// Searches the bar places retrieved.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="result">The result.</param>
        void Search_Bar_PlacesRetrieved(object sender, AutoCompleteResult result)
        {
             (this.BindingContext as AddCityViewModel)?.OnPlacesRetrieved.Execute(result);
        }

        /// <summary>
        /// Handles the TextChanged event of the Search_Bar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs"/> instance containing the event data.</param>
        void Search_Bar_TextChanged(object sender, TextChangedEventArgs e)
        {
            (this.BindingContext as AddCityViewModel)?.OnTextChanged.Execute(e.NewTextValue);
        }

        /// <summary>
        /// Handles the ItemSelected event of the Results_List control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectedItemChangedEventArgs"/> instance containing the event data.</param>
        void Results_List_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;
            (this.BindingContext as AddCityViewModel)?.OnListItemSelected.Execute(e.SelectedItem);
        }

        /// <summary>
        /// Clickeds the asynchronous.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        async void ClickedAsync(object sender, EventArgs e)
        {
            var viewModel = this.BindingContext as AddCityViewModel;
            if (viewModel.SelectedCity != null)
            {
                MessagingCenter.Send<AddCityPage, NamedCity>(this, "add", viewModel.SelectedCity);
                await Navigation.PopAsync();
            }
        }

    }
}