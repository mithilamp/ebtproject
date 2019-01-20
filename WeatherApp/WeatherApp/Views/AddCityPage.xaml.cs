using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeatherApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddCityPage : ContentPage
	{
        string GooglePlacesApiKey = "AIzaSyCfedRBIYQWmXq2jSB_QDCvGGM62fEoTZE";  // Replace this with your api key
        NamedCity selectedCity { get; set; }

        public AddCityPage ()
		{
			InitializeComponent ();
            SearchBar.ApiKey = GooglePlacesApiKey;
            SearchBar.Type = PlaceType.Geocode;
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
            results_list.ItemsSource = result.AutoCompletePlaces;
            spinner.IsRunning = false;
            spinner.IsVisible = false;

            if (result.AutoCompletePlaces != null && result.AutoCompletePlaces.Count > 0)
                results_list.IsVisible = true;
        }

        /// <summary>
        /// Handles the TextChanged event of the Search_Bar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs"/> instance containing the event data.</param>
        void Search_Bar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewTextValue))
            {
                results_list.IsVisible = false;
                spinner.IsVisible = true;
                spinner.IsRunning = true;
            }
            else
            {
                results_list.IsVisible = true;
                spinner.IsRunning = false;
                spinner.IsVisible = false;
            }
        }

        /// <summary>
        /// Handles the ItemSelected event of the Results_List control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectedItemChangedEventArgs"/> instance containing the event data.</param>
        async void Results_List_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            var prediction = (AutoCompletePrediction)e.SelectedItem;
            SearchBar.Text = prediction.Description;
            results_list.SelectedItem = null;

            this.selectedCity = await Places.GetPlace(prediction.Place_ID, GooglePlacesApiKey);
        }

        async void ClickedAsync(object sender, EventArgs e)
        {
            if (this.selectedCity != null)
            {
                MessagingCenter.Send<AddCityPage, NamedCity>(this, "add", selectedCity);
                await Navigation.PopAsync();
            }
        }

    }
}