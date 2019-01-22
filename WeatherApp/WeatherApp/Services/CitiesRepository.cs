using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Interfaces;
using WeatherApp.Models;

namespace WeatherApp.Services
{
    public class CitiesRepository : ICitiesRepository
    {
        /// <summary>
        /// The database context
        /// </summary>
        private readonly DatabaseContext databaseContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="CitiesRepository"/> class.
        /// </summary>
        /// <param name="dbPath">The database path.</param>
        public CitiesRepository(string dbPath)
        {
            databaseContext = new DatabaseContext(dbPath);
        }

        /// <summary>
        /// Gets the cities asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<NamedCity>> GetCitiesAsync()
        {
            try
            {
                var cities = await databaseContext.Cities.ToListAsync();
                return cities;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Adds the city asynchronous.
        /// </summary>
        /// <param name="city">The city.</param>
        /// <returns></returns>
        public async Task<bool> AddCityAsync(NamedCity city)
        {
            try
            {
                var tracking = await databaseContext.Cities.AddAsync(city);
                await databaseContext.SaveChangesAsync();
                var isAdded = tracking.State == EntityState.Added;
                return isAdded;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the city by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<NamedCity> GetCityByIdAsync(int id)
        {
            try
            {
                var city = await databaseContext.Cities.FindAsync(id);
                return city;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Removes the city asynchronous.
        /// </summary>
        /// <param name="city">The city.</param>
        /// <returns></returns>
        public async Task<bool> RemoveCityAsync(NamedCity city)
        {
            try
            {
                var tracking = databaseContext.Cities.Remove(city);
                await databaseContext.SaveChangesAsync();
                var isDeleted = tracking.State == EntityState.Deleted;
                return isDeleted;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Updates the city asynchronous.
        /// </summary>
        /// <param name="city">The city.</param>
        /// <returns></returns>
        public async Task<bool> UpdateCityAsync(NamedCity city)
        {
            try
            {
                var tracking = databaseContext.Update(city);
                await databaseContext.SaveChangesAsync();
                var isModified = tracking.State == EntityState.Modified;
                return isModified;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
