using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Models;
using Xamarin.Forms;

namespace WeatherApp.Controls
{
    public delegate void PlacesRetrievedEventHandler(object sender, AutoCompleteResult result);

    public class ExtendedPlaceSearchBar : SearchBar
    {
        public static readonly BindableProperty PlaceTypeProperty = BindableProperty.Create(nameof(Type), typeof(PlaceType), typeof(ExtendedPlaceSearchBar), PlaceType.All, BindingMode.OneWay, (BindableProperty.ValidateValueDelegate)null, (BindableProperty.BindingPropertyChangedDelegate)null, (BindableProperty.BindingPropertyChangingDelegate)null, (BindableProperty.CoerceValueDelegate)null, (BindableProperty.CreateDefaultValueDelegate)null);

        /// <summary>
        /// Backing store for the ApiKey property.
        /// </summary>
        public static readonly BindableProperty ApiKeyProperty = BindableProperty.Create(nameof(ApiKey), typeof(string), typeof(ExtendedPlaceSearchBar), string.Empty, BindingMode.OneWay, (BindableProperty.ValidateValueDelegate)null, (BindableProperty.BindingPropertyChangedDelegate)null, (BindableProperty.BindingPropertyChangingDelegate)null, (BindableProperty.CoerceValueDelegate)null, (BindableProperty.CreateDefaultValueDelegate)null);

        /// <summary>
        /// Backing store for the MinimumSearchText property.
        /// </summary>
        public static readonly BindableProperty MinimumSearchTextProperty = BindableProperty.Create(nameof(MinimumSearchText), typeof(int), typeof(ExtendedPlaceSearchBar), 2, BindingMode.OneWay, (BindableProperty.ValidateValueDelegate)null, (BindableProperty.BindingPropertyChangedDelegate)null, (BindableProperty.BindingPropertyChangingDelegate)null, (BindableProperty.CoerceValueDelegate)null, (BindableProperty.CreateDefaultValueDelegate)null);

        #region Property accessors
        /// <summary>
        /// Gets or sets the place type.
        /// </summary>
        /// <value>The type.</value>
        public PlaceType Type
        {
            get
            {
                return (PlaceType)this.GetValue(ExtendedPlaceSearchBar.PlaceTypeProperty);
            }
            set
            {
                this.SetValue(ExtendedPlaceSearchBar.PlaceTypeProperty, (object)value);
            }
        }


        /// <summary>
        /// Gets or sets the API key.
        /// </summary>
        /// <value>The API key.</value>
        public string ApiKey
        {
            get
            {
                return (string)this.GetValue(ExtendedPlaceSearchBar.ApiKeyProperty);

            }
            set
            {
                this.SetValue(ExtendedPlaceSearchBar.ApiKeyProperty, (object)value);
            }
        }

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
                var predictions = await GetPlaces(e.NewTextValue);
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

        /// <summary>
        /// Calls the Google Places API to retrieve autofill suggestions
        /// </summary>
        /// <returns>The places.</returns>
        /// <param name="newTextValue">New text value.</param>
        async Task<AutoCompleteResult> GetPlaces(string newTextValue)
        {
            if (string.IsNullOrEmpty(ApiKey))
            {
                throw new Exception(
                    string.Format("You have not assigned a Google API key to PlacesBar"));
            }

            try
            {
                var requestURI = CreatePredictionsUri(newTextValue);
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, requestURI);
                var response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("PlacesBar HTTP request denied.");
                    return null;
                }

                var result = await response.Content.ReadAsStringAsync();

                if (result == "ERROR")
                {
                    Debug.WriteLine("PlacesSearchBar Google Places API returned ERROR");
                    return null;
                }

                return JsonConvert.DeserializeObject<AutoCompleteResult>(result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("PlacesBar HTTP issue: {0} {1}", ex.Message, ex);
                return null;
            }
        }

        /// <summary>
        /// Creates the predictions URI.
        /// </summary>
        /// <returns>The predictions URI.</returns>
        /// <param name="newTextValue">New text value.</param>
        string CreatePredictionsUri(string newTextValue)
        {
            var url = "https://maps.googleapis.com/maps/api/place/autocomplete/json";
            var input = Uri.EscapeUriString(newTextValue);
            var pType = PlaceTypeValue(Type);
            var constructedUrl = $"{url}?input={input}&types={pType}&key={ApiKey}";

            return constructedUrl;
        }

        /// <summary>
        /// Returns a string representation of the specified place type.
        /// </summary>
        /// <returns>The type value.</returns>
        /// <param name="type">Type.</param>
        string PlaceTypeValue(PlaceType type)
        {
            switch (type)
            {
                case PlaceType.All:
                    return "";
                case PlaceType.Geocode:
                    return "geocode";
                case PlaceType.Address:
                    return "address";
                case PlaceType.Establishment:
                    return "establishment";
                case PlaceType.Regions:
                    return "(regions)";
                case PlaceType.Cities:
                    return "(cities)";
                default:
                    return "";
            }
        }

    }

}
