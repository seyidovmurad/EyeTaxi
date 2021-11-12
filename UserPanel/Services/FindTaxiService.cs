using AdminPanel.Models;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserPanel.Services
{
    public class FindTaxiService
    {

        public static Location TaxiLocation(Location origin,List<Location> drivers,float maxRadius = 10) //Throw exception if no car found in given radius
        {
            float radius = 0.5f; //500m 
            while(radius < maxRadius)
            {
                var radPerDeg = Math.PI / 180;
                var earthRadius = 6371; // in km
                var lat = origin.Latitude * radPerDeg;
                var lon = origin.Longitude * radPerDeg;
                var AngDist = radius / earthRadius;
                for (int i = 0; i <= 360; i++)
                {
                    double pLatidue, pLongitude;
                    double point = i * radPerDeg;
                    pLatidue = Math.Asin(Math.Sin(lat) * Math.Cos(AngDist) + Math.Cos(lat) * Math.Sin(AngDist) * Math.Cos(point));
                    pLongitude = lon + Math.Atan2(Math.Sin(point) * Math.Sin(AngDist) * Math.Cos(lat), Math.Cos(AngDist) - Math.Sin(lat) * Math.Sin(pLatidue));
                    pLatidue /= radPerDeg;
                    pLongitude /= radPerDeg;
                    foreach (var driver in drivers) 
                    {
                        double carLat = Math.Abs(origin.Latitude - driver.Latitude);
                        double carLong = Math.Abs(origin.Longitude - driver.Longitude);
                        double circleLat = Math.Abs(origin.Latitude - pLatidue);
                        double circleLong = Math.Abs(origin.Longitude - pLongitude);
                        if (carLong < circleLong && carLat < circleLat)
                        {
                            return new Location(pLatidue, pLongitude);
                        }
                    }
                }
                radius += 0.5f; //increase radius 500m
            }
            throw new Exception($"There is no car {maxRadius} radius");
        }


        public static Location FindTaxiByDistance(Location origin,List<Location> drivers)
        {
            Location nearestLocation = new Location();
            double distance1,distance2 = 0;
            foreach (var driver in drivers)
            {
                distance1 = GetDistanceBetweenPoints(origin.Latitude, origin.Longitude, driver.Latitude, driver.Longitude);
                if(distance1 > distance2)
                {
                    distance2 = distance1;
                    nearestLocation = driver;
                }
            }
            return nearestLocation;
        }

        public static double GetDistanceBetweenPoints(double lat1, double long1, double lat2, double long2)
        {
            double distance = 0;

            double dLat = (lat2 - lat1) / 180 * Math.PI;
            double dLong = (long2 - long1) / 180 * Math.PI;

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2)
                        + Math.Cos(lat1 / 180 * Math.PI) * Math.Cos(lat2 / 180 * Math.PI)
                        * Math.Sin(dLong / 2) * Math.Sin(dLong / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            //Calculate radius of earth
            // For this you can assume any of the two points.
            double radiusE = 6378135; // Equatorial radius, in metres
            double radiusP = 6356750; // Polar Radius

            //Numerator part of function
            double nr = Math.Pow(radiusE * radiusP * Math.Cos(lat1 / 180 * Math.PI), 2);
            //Denominator part of the function
            double dr = Math.Pow(radiusE * Math.Cos(lat1 / 180 * Math.PI), 2)
                            + Math.Pow(radiusP * Math.Sin(lat1 / 180 * Math.PI), 2);
            double radius = Math.Sqrt(nr / dr);

            //Calculate distance in meters.
            distance = radius * c;
            return distance; // distance in meters
        }
    }
}
