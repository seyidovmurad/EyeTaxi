﻿using AdminPanel.Models;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserPanel.Services
{
    public class FindTaxiService
    {

        //public static Location TaxiLocation(Location origin,List<Location> drivers,float maxRadius = 10) //Throw exception if no car found in given radius
        //{
        //    float radius = 0.5f; //500m 
        //    while(radius < maxRadius)
        //    {
        //        var radPerDeg = Math.PI / 180;
        //        var earthRadius = 6371; // in km
        //        var lat = origin.Latitude * radPerDeg;
        //        var lon = origin.Longitude * radPerDeg;
        //        var AngDist = radius / earthRadius;
        //        for (int i = 0; i <= 360; i++)
        //        {
        //            double pLatidue, pLongitude;
        //            double point = i * radPerDeg;
        //            pLatidue = Math.Asin(Math.Sin(lat) * Math.Cos(AngDist) + Math.Cos(lat) * Math.Sin(AngDist) * Math.Cos(point));
        //            pLongitude = lon + Math.Atan2(Math.Sin(point) * Math.Sin(AngDist) * Math.Cos(lat), Math.Cos(AngDist) - Math.Sin(lat) * Math.Sin(pLatidue));
        //            pLatidue /= radPerDeg;
        //            pLongitude /= radPerDeg;
        //            foreach (var driver in drivers) 
        //            {
        //                double carLat = Math.Abs(origin.Latitude - driver.Latitude);
        //                double carLong = Math.Abs(origin.Longitude - driver.Longitude);
        //                double circleLat = Math.Abs(origin.Latitude - pLatidue);
        //                double circleLong = Math.Abs(origin.Longitude - pLongitude);
        //                if (carLong < circleLong && carLat < circleLat)
        //                {
        //                    return new Location(pLatidue, pLongitude);
        //                }
        //            }
        //        }
        //        radius += 0.5f; //increase radius 500m
        //    }
        //    throw new Exception($"There is no car {maxRadius} radius");
        //}


        public static Driver TaxiLocation(Location origin,List<Driver> drivers)
        {
            Location nearestLocation = new Location();
            double distance1 = 0,distance2 = 0;
            var driverCopy = new Driver();
            foreach (var driver in drivers)
            {
                GeoCoordinate geo = new GeoCoordinate(origin.Latitude, origin.Longitude);

                distance1 = geo.GetDistanceTo(new GeoCoordinate(driver.LastLocation.Latitude, driver.LastLocation.Longitude));
                if(distance1 < distance2 || distance2 == 0)
                {
                    driverCopy = driver;
                    distance2 = distance1;
                }
            }
            if (distance2 > 5000)
                throw new Exception("Taxi not found");
            return driverCopy;
        }

        
    }
}
