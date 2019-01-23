using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Models;

namespace WeatherApp.Interfaces
{
    interface IGoogleMapServices
    {
        /// <summary>
        /// Gets the place.
        /// </summary>
        /// <param name="placeID">The place identifier.</param>
        /// <returns></returns>
        Task<NamedCity> GetPlace(string placeID);
        /// <summary>
        /// Gets the places.
        /// </summary>
        /// <param name="newTextValue">The new text value.</param>
        /// <returns></returns>
        Task<AutoCompleteResult> GetPlaces(string newTextValue);

        /// <summary>
        /// Gets the reverse geocoding place.
        /// </summary>
        /// <param name="lat">The lat.</param>
        /// <param name="lon">The lon.</param>
        /// <returns></returns>
        Task<NamedCity> GetReverseGeocodingPlace(double lat, double lon);
    }
}
