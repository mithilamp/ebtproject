using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Interfaces;
using WeatherApp.Models;
using Xamarin.Forms;

namespace WeatherApp.Services
{
    public class RestServices : IRestServices
    {
        /// <summary>
        /// Gets or sets the name of the city.
        /// </summary>
        private string CityName {get;set;}

        /// <summary>
        /// The client
        /// </summary>
        HttpClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="RestServices"/> class.
        /// </summary>
        public RestServices()
        {
            client = new HttpClient();
        }

        /// <summary>
        /// Changes the city.
        /// </summary>
        /// <param name="city">The city.</param>
        public void ChangeCity(string city)
        {
            this.CityName = city;
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <returns></returns>
        public async Task<WeatherData> GetWeatherData()
        {
            WeatherData weatherData = null;
            try
            {
                var response = await client.GetAsync(GenerateRequestUri(Constants.OpenWeatherMapEndpoint));
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    weatherData = JsonConvert.DeserializeObject<WeatherData>(content);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage?.DisplayAlert("Error", ex.Message, "Ok");
            }

            return weatherData;
        }

        /// <summary>
        /// Generates the request URI.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <returns></returns>
        string GenerateRequestUri(string endpoint)
        {
            string requestUri = endpoint;
            requestUri += $"?q={this.CityName}";
            requestUri += "&units=metric";
            requestUri += $"&APPID={Constants.OpenWeatherMapAPIKey}";
            return requestUri;
        }
    }
}
