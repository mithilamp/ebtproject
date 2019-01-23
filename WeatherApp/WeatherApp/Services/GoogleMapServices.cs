using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Interfaces;
using WeatherApp.Models;
using Xamarin.Forms;

namespace WeatherApp.Services
{
    public class GoogleMapServices : IGoogleMapServices
    {
        public GoogleMapServices()
        {

        }

        public async Task<NamedCity> GetReverseGeocodingPlace(double lat,double lon)
        {
            try
            {
                var requestURI = CreateReverseGeocodeUri(lat,lon);
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

                var namedCity = new NamedCity();
                namedCity.Name = (string)JObject.Parse(result)["results"][0]["address_components"][3]["short_name"];
                namedCity.Latitude = lat;
                namedCity.Longitude = lon;
                return namedCity;

            }
            catch (Exception ex)
            {
                Debug.WriteLine("PlacesBar HTTP issue: {0} {1}", ex.Message, ex);
                return null;
            }
        }

        private string CreateReverseGeocodeUri(double lat, double lon)
        {
            var url = Constants.GoogleReverseGeocodingEndPoint;
            return $"{url}?latlng={lat},{lon}&location_type=ROOFTOP&result_type=street_address&key={Constants.GoogleApiKey}";
        }

        /// <summary>
        /// Gets the place.
        /// </summary>
        /// <param name="placeID">The place identifier.</param>
        /// <returns></returns>
        public async Task<NamedCity> GetPlace(string placeID)
        {
            try
            {
                var requestURI = CreateDetailsRequestUri(placeID);
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
                var namedCity = new NamedCity();
                namedCity.Name = (string)JObject.Parse(result)["result"]["name"];
                namedCity.Latitude = (double)JObject.Parse(result)["result"]["geometry"]["location"]["lat"];
                namedCity.Longitude = (double)JObject.Parse(result)["result"]["geometry"]["location"]["lng"];
                return namedCity;

            }
            catch (Exception ex)
            {
                Debug.WriteLine("PlacesBar HTTP issue: {0} {1}", ex.Message, ex);
                return null;
            }
        }

        /// <summary>
        /// Gets the places.
        /// </summary>
        /// <param name="newTextValue">The new text value.</param>
        /// <returns></returns>
        public async Task<AutoCompleteResult> GetPlaces(string newTextValue)
        {
            if (string.IsNullOrEmpty(Constants.GoogleApiKey))
            {
                await Application.Current.MainPage?.DisplayAlert("Error", "You have not assigned a Google API key", "Ok");
                return null;
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
        private string CreatePredictionsUri(string newTextValue)
        {
            var url = Constants.GooglePlacesPredictionEndpoint;
            var input = Uri.EscapeUriString(newTextValue);
            var pType = PlaceTypeValue(PlaceType.Geocode);
            var constructedUrl = $"{url}?input={input}&types={pType}&key={Constants.GoogleApiKey}";

            return constructedUrl;
        }

        /// <summary>
        /// Creates the details request URI.
        /// </summary>
        /// <param name="place_id">The place identifier.</param>
        /// <param name="apiKey">The API key.</param>
        /// <returns></returns>
        private string CreateDetailsRequestUri(string place_id)
        {
            var url = Constants.GooglePlacesDetailsEndpoint;
            return $"{url}?placeid={Uri.EscapeUriString(place_id)}&key={Constants.GoogleApiKey}";
        }

        /// <summary>
        /// Returns a string representation of the specified place type.
        /// </summary>
        /// <returns>The type value.</returns>
        /// <param name="type">Type.</param>
        private string PlaceTypeValue(PlaceType type)
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
