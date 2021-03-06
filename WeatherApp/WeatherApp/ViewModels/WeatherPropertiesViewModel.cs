﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeatherApp.Models;

namespace WeatherApp.ViewModels
{
    public class WeatherPropertiesViewModel :BaseViewModel
    {
        private string cityName;
        private string visibility;
        private string icon;
        private double temp;
        private DateTime dtTxt;
        private double speed;
        private long humidity;
        private double pressure;

        /// <summary>
        /// The forcast list
        /// </summary>
        IList<List> forcastList;

        public WeatherPropertiesViewModel()
        {

        }

        /// <summary>
        /// Generates the new data.
        /// </summary>
        /// <param name="weatherData">The weather data.</param>
        public void GenerateNewData(WeatherData weatherData)
        {
            this.CityName = weatherData.City.Name;
            //get latest weather updates
            this.Visibility = weatherData.List[0].Weather[0].Visibility;
            this.Icon = weatherData.List[0].Weather[0].Icon;
            this.Temparature = weatherData.List[0].Main.Temp;
            this.DtTxt = weatherData.List[0].DtTxt;
            this.Speed = weatherData.List[0].Wind.Speed;
            this.Humidity = weatherData.List[0].Main.Humidity;
            this.Pressure = weatherData.List[0].Main.Pressure;

            var timeOfDay = weatherData.List[0].DtTxt.TimeOfDay; //latest weather update
            this.ForcastList = weatherData.List.Where(x => x.DtTxt.TimeOfDay == timeOfDay).ToList();
        }

        /// <summary>
        /// Populates the header data.
        /// </summary>
        /// <param name="forcast">The forcast.</param>
        public void PopulateHeaderData(List forcast)
        {
            this.Visibility = forcast.Weather[0].Visibility;
            this.Icon = forcast.Weather[0].Icon;
            this.Temparature = forcast.Main.Temp;
            this.DtTxt = forcast.DtTxt;
            this.Speed = forcast.Wind.Speed;
            this.Humidity = forcast.Main.Humidity;
            this.Pressure = forcast.Main.Pressure;
        }

        /// <summary>
        /// Gets or sets the forcast list.
        /// </summary>
        public IList<List> ForcastList
        {
            get { return forcastList; }
            set
            {
                if (forcastList != value)
                {
                    forcastList = value;
                    //forcastList.RemoveAt(0); //Exempt today
                    OnPropertyChanged("ForcastList");
                }
            }
        }

        /// <summary>
        /// Gets or sets the name of the city.
        /// </summary>
        public string CityName
        {
            get { return this.cityName; }
            set
            {
                if (this.cityName != value)
                {
                    this.cityName = value;
                    OnPropertyChanged("CityName");
                }
            }
        }

        /// <summary>
        /// Gets or sets the visibility.
        /// </summary>
        public string Visibility
        {
            get { return this.visibility; }
            set
            {
                if (this.visibility != value)
                {
                    this.visibility = value;
                    OnPropertyChanged("Visibility");
                }
            }
        }

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        public string Icon
        {
            get { return this.icon; }
            set
            {
                if (this.icon != value)
                {
                    this.icon = value;
                    OnPropertyChanged("Icon");
                }
            }
        }

        /// <summary>
        /// Gets or sets the Temparature.
        /// </summary>
        public double Temparature
        {
            get { return this.temp; }
            set
            {
                if (this.temp != value)
                {
                    this.temp = value;
                    OnPropertyChanged("Temparature");
                }
            }
        }

        /// <summary>
        /// Gets or sets the dt text.
        /// </summary>
        public DateTime DtTxt
        {
            get { return this.dtTxt; }
            set
            {
                if (this.dtTxt != value)
                {
                    this.dtTxt = value;
                    OnPropertyChanged("DtTxt");
                }
            }
        }

        /// <summary>
        /// Gets or sets the speed.
        /// </summary>
        public double Speed
        {
            get { return this.speed; }
            set
            {
                if (this.speed != value)
                {
                    this.speed = value;
                    OnPropertyChanged("Speed");
                }
            }
        }

        /// <summary>
        /// Gets or sets the humidity.
        /// </summary>
        public long Humidity
        {
            get { return this.humidity; }
            set
            {
                if (this.humidity != value)
                {
                    this.humidity = value;
                    OnPropertyChanged("Humidity");
                }
            }
        }

        /// <summary>
        /// Gets or sets the pressure.
        /// </summary>
        public double Pressure
        {
            get { return this.pressure; }
            set
            {
                if (this.pressure != value)
                {
                    this.pressure = value;
                    OnPropertyChanged("Pressure");
                }
            }
        }

    }
}
