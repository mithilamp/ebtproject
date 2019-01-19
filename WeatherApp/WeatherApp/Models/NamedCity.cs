using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherApp.Models
{
    public class NamedCity
    {
        public NamedCity(string name, double lon, double lat)
        {
            this.Name = name;
            this.Longitude = lon;
            this.Latitude = lat;
        }

        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
