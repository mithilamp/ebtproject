using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Models;

namespace WeatherApp
{
    public class Places
    {
        /// <summary>
        /// Gets the place.
        /// </summary>
        /// <returns>The place.</returns>
        /// <param name="placeID">Place identifier.</param>
        /// <param name="apiKey">API key.</param>
        public static async Task<NamedCity> GetPlace(string placeID, string apiKey)
        {
            try
            {
                var requestURI = CreateDetailsRequestUri(placeID, apiKey);
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
                //var namedCity = new NamedCity();
                return namedCity;

            }
            catch (Exception ex)
            {
                Debug.WriteLine("PlacesBar HTTP issue: {0} {1}", ex.Message, ex);
                return null;
            }
        }

        /// <summary>
        /// Creates the details request URI.
        /// </summary>
        /// <param name="place_id">The place identifier.</param>
        /// <param name="apiKey">The API key.</param>
        /// <returns></returns>
        static string CreateDetailsRequestUri(string place_id, string apiKey)
        {
            var url = "https://maps.googleapis.com/maps/api/place/details/json";
            return $"{url}?placeid={Uri.EscapeUriString(place_id)}&key={apiKey}";
        }
    }
}
