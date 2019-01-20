using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherApp.Models
{
    public class NamedCity
    {
        public NamedCity(double lon, double lat, string name = null)
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
