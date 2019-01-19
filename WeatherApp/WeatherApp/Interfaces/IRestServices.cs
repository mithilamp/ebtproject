using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Models;

namespace WeatherApp.Interfaces
{
    public interface IRestServices
    {
        /// <summary>
        /// Gets the weather data.
        /// </summary>
        /// <returns></returns>
        Task<WeatherData> GetWeatherData(NamedCity city);
    }
}
