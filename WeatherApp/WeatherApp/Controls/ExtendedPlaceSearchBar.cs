using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Interfaces;
using WeatherApp.Models;
using WeatherApp.Services;
using Xamarin.Forms;

namespace WeatherApp.Controls
{
    public delegate void PlacesRetrievedEventHandler(object sender, AutoCompleteResult result);

    public class ExtendedPlaceSearchBar : SearchBar
    {
        /// <summary>
        /// Backing store for the MinimumSearchText property.
        /// </summary>
        public static readonly BindableProperty MinimumSearchTextProperty = BindableProperty.Create(nameof(MinimumSearchText),
            typeof(int), 
            typeof(ExtendedPlaceSearchBar), 
            2, 
            BindingMode.OneWay);

        #region Property accessors

        /// <summary>
        /// Gets or sets the minimum search text.
        /// </summary>
        /// <value>The minimum search text.</value>
        public int MinimumSearchText
        {
            get
            {
                return (int)this.GetValue(ExtendedPlaceSearchBar.MinimumSearchTextProperty);

            }
            set
            {
                this.SetValue(ExtendedPlaceSearchBar.MinimumSearchTextProperty, (object)value);
            }
        }
        #endregion

        /// <summary>
        /// The places retrieved handler.
        /// </summary>
        public event PlacesRetrievedEventHandler PlacesRetrieved;

        /// <summary>
        /// The google map services
        /// </summary>
        private IGoogleMapServices googleMapServices;

        /// <summary>
        /// Called when [places retrieved].
        /// </summary>
        /// <param name="e">The e.</param>
        protected virtual void OnPlacesRetrieved(AutoCompleteResult e)
        {
            PlacesRetrievedEventHandler handler = PlacesRetrieved;
            handler?.Invoke(this, e);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:DurianCode.PlacesSearchBar.PlacesBar"/> class.
        /// </summary>
        public ExtendedPlaceSearchBar()
        {
            googleMapServices = new GoogleMapServices();
            TextChanged += OnTextChanged;
        }

        /// <summary>
        /// Handles changes to search text.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        async void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewTextValue) && e.NewTextValue.Length >= MinimumSearchText)
            {
                var predictions = await googleMapServices.GetPlaces(e.NewTextValue);
                if (PlacesRetrieved != null && predictions != null)
                    OnPlacesRetrieved(predictions);
                else
                    OnPlacesRetrieved(new AutoCompleteResult());
            }
            else
            {
                OnPlacesRetrieved(new AutoCompleteResult());
            }
        }

    }

}
