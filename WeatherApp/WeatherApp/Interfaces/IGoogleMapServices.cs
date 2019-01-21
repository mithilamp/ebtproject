using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Models;

namespace WeatherApp.Interfaces
{
    interface IGoogleMapServices
    {
        Task<NamedCity> GetPlace(string placeID);
        Task<AutoCompleteResult> GetPlaces(string newTextValue);
    }
}
