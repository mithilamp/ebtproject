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
        private readonly DatabaseContext databaseContext;

        public CitiesRepository(string dbPath)
        {
            databaseContext = new DatabaseContext(dbPath);
        }

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
