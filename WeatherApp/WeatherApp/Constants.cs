using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherApp
{
    public class Constants
    {
        public static string OpenWeatherMapEndpoint = "https://api.openweathermap.org/data/2.5/forecast";
        public static string OpenWeatherMapAPIKey = "01b14eddded68c3d739c012290769c48";

        public static string GoogleApiKey = "";
        public static string GooglePlacesPredictionEndpoint = "https://maps.googleapis.com/maps/api/place/autocomplete/json";
        public static string GooglePlacesDetailsEndpoint = "https://maps.googleapis.com/maps/api/place/details/json";
        public static string GoogleReverseGeocodingEndPoint = "https://maps.googleapis.com/maps/api/geocode/json";
    }
}
