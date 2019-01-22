using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Models;

namespace WeatherApp.Interfaces
{
    public interface ICitiesRepository
    {
        /// <summary>
        /// Gets the cities asynchronous.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<NamedCity>> GetCitiesAsync();
        /// <summary>
        /// Gets the city by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<NamedCity> GetCityByIdAsync(int id);
        /// <summary>
        /// Adds the city asynchronous.
        /// </summary>
        /// <param name="city">The city.</param>
        /// <returns></returns>
        Task<bool> AddCityAsync(NamedCity city);
        /// <summary>
        /// Updates the city asynchronous.
        /// </summary>
        /// <param name="city">The city.</param>
        /// <returns></returns>
        Task<bool> UpdateCityAsync(NamedCity city);
        /// <summary>
        /// Removes the city asynchronous.
        /// </summary>
        /// <param name="city">The city.</param>
        /// <returns></returns>
        Task<bool> RemoveCityAsync(NamedCity city);
    }
}
