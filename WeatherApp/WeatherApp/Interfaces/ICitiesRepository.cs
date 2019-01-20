using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Models;

namespace WeatherApp.Interfaces
{
    public interface ICitiesRepository
    {
        Task<IEnumerable<NamedCity>> GetCitiesAsync();
        Task<NamedCity> GetCityByIdAsync(int id);
        Task<bool> AddCityAsync(NamedCity city);
        Task<bool> UpdateCityAsync(NamedCity city);
        Task<bool> RemoveCityAsync(NamedCity city);
    }
}
