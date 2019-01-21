using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Interfaces;
using WeatherApp.Models;
using WeatherApp.Services;
using Xamarin.Forms;

namespace WeatherApp.ViewModels
{
    public class AddCityViewModel:BaseViewModel
    {
        /// <summary>
        /// Gets or sets the selected city.
        /// </summary>
        public NamedCity SelectedCity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [not selected].
        /// </summary>
        bool NotSelected { get; set; }

        /// <summary>
        /// The is list visible
        /// </summary>
        private bool isListVisible;

        /// <summary>
        /// The search text
        /// </summary>
        private string searchText;

        /// <summary>
        /// The google map services
        /// </summary>
        private IGoogleMapServices googleMapServices;

        /// <summary>
        /// The on places retrieved
        /// </summary>
        private Command onPlacesRetrieved;

        /// <summary>
        /// The on text changed
        /// </summary>
        private Command onTextChanged;

        /// <summary>
        /// The on list item selected
        /// </summary>
        private Command onListItemSelected;

        /// <summary>
        /// The automatic complete predictions
        /// </summary>
        private List<AutoCompletePrediction> autoCompletePredictions;

        /// <summary>
        /// Gets the on places retrieved.
        /// </summary>
        public Command OnPlacesRetrieved { get => this.onPlacesRetrieved ?? (this.onPlacesRetrieved = new Command((e) => this.OnPlacesRetrievedAction(e as AutoCompleteResult))); }

        /// <summary>
        /// Gets the on text changed.
        /// </summary>
        public Command OnTextChanged { get => this.onTextChanged ?? (this.onTextChanged = new Command((e) => this.OnTextChangedAction(e as string))); }

        /// <summary>
        /// Gets the on list item selected.
        /// </summary>
        public Command OnListItemSelected { get => this.onListItemSelected ?? (this.onListItemSelected = new Command(async (e) => await this.OnListItemSelectedActionAsync(e as AutoCompletePrediction))); }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is list visible.
        /// </summary>
        public bool IsListVisible
        {
            get { return this.isListVisible; }
            set
            { 
                if (isListVisible != value)
                {
                    isListVisible = value;
                    OnPropertyChanged("IsListVisible");
                }
            }
        }

        /// <summary>
        /// Gets or sets the search text.
        /// </summary>
        public string SearchText
        {
            get { return this.searchText; }
            set
            {
                if (searchText != value)
                {
                    searchText = value;
                    OnPropertyChanged("SearchText");
                }
            }
        }

        /// <summary>
        /// Gets or sets the automatic complete predictions.
        /// </summary>
        public List<AutoCompletePrediction> AutoCompletePredictions
        {
            get { return this.autoCompletePredictions; }
            set
            {
                if (autoCompletePredictions != value)
                {
                    autoCompletePredictions = value;
                    OnPropertyChanged("AutoCompletePredictions");
                }
            }
        }

        /// <summary>
        /// Called when [list item selected action asynchronous].
        /// </summary>
        /// <param name="autoCompletePrediction">The automatic complete prediction.</param>
        /// <returns></returns>
        private async Task OnListItemSelectedActionAsync(AutoCompletePrediction autoCompletePrediction)
        {
            SearchText = autoCompletePrediction.Description;
            this.SelectedCity = await googleMapServices.GetPlace(autoCompletePrediction.Place_ID);
        }

        /// <summary>
        /// Called when [text changed action].
        /// </summary>
        /// <param name="text">The text.</param>
        private void OnTextChangedAction(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                IsListVisible = false;
            }
            else
            {
                IsListVisible = true;
                this.SelectedCity = null;
            }
        }

        /// <summary>
        /// Called when [places retrieved action].
        /// </summary>
        /// <param name="autoCompletePrediction">The automatic complete prediction.</param>
        private void OnPlacesRetrievedAction(AutoCompleteResult autoCompletePrediction)
        {
            AutoCompletePredictions = autoCompletePrediction.AutoCompletePlaces;

            if (autoCompletePrediction.AutoCompletePlaces != null && autoCompletePrediction.AutoCompletePlaces.Count > 0)
                IsListVisible = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddCityViewModel"/> class.
        /// </summary>
        public AddCityViewModel()
        {
            NotSelected = true;
            googleMapServices = new GoogleMapServices();
            this.SelectedCity = new NamedCity();
            AutoCompletePredictions = new List<AutoCompletePrediction>();
            IsListVisible = false;
        }
    }
}
