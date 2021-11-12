using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserPanel.Services;

namespace AdminPanel.Services
{
    public class RandomLocationService
    {

        public static Location RandomLocation()
        {
            Location origin = new Location(40.3791, 49.8468);
            var radPerDeg = Math.PI / 180;
            var earthRadius = 6371; // in km
            var lat = origin.Latitude * radPerDeg;
            var lon = origin.Longitude * radPerDeg;
            var AngDist = 10f / earthRadius;
            Random random = new Random();
            int deg = random.Next(0, 360);
            double pLatidue, pLongitude;
            double point = deg * radPerDeg;
            pLatidue = Math.Asin(Math.Sin(lat) * Math.Cos(AngDist) + Math.Cos(lat) * Math.Sin(AngDist) * Math.Cos(point));
            pLongitude = lon + Math.Atan2(Math.Sin(point) * Math.Sin(AngDist) * Math.Cos(lat), Math.Cos(AngDist) - Math.Sin(lat) * Math.Sin(pLatidue));
            pLatidue /= radPerDeg;
            pLongitude /= radPerDeg;
            LocationCollection locations = new LocationCollection();
            GetRouteService.GetRoute("40.3791, 49.8468", $"{pLatidue}, {pLongitude}", locations);
            deg = random.Next(0, locations.Count - 1);
            return locations.ElementAt(deg);
        }
    }
}
