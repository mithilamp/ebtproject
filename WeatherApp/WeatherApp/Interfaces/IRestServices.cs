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
        /// Changes the city.
        /// </summary>
        /// <param name="city">The city.</param>
        void ChangeCity(string city);

        /// <summary>
        /// Gets the weather data.
        /// </summary>
        /// <returns></returns>
        Task<WeatherData> GetWeatherData();
    }
}
