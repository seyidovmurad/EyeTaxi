using AdminPanel.Models;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using static Bogus.DataSets.Name;

namespace AdminPanel.Services
{

    class FakeDriverService
    {


        
        public static Driver GenerateDriver()
        {
            var driverFaker = new Faker<Driver>("az")
                .RuleFor(d => d.Name, f => f.Name.FirstName(Gender.Male))
                .RuleFor(d => d.Surname, f => f.Name.LastName(Gender.Male))
                .RuleFor(d => d.PhoneNumber, f => f.Phone.PhoneNumber())
                .RuleFor(d => d.Balance, f => f.Random.Number(20, 300));
            var driver = driverFaker.Generate();
            driver.RatingCount = 0;
            driver.RatingSum = 0;
            driver.Rating = "0";

            driver.Car = GenerateCar();
            driver.Car.Color = "Black";
            driver.LastLocation = RandomLocationService.RandomLocation();
            return driver;
        }

        public static Car GenerateCar()
        {
            var carFaker = new Faker<Car>()
                .RuleFor(c => c.Vendor, f => f.Vehicle.Manufacturer())
                .RuleFor(c => c.Model, f => f.Vehicle.Model())
                .RuleFor(c => c.Year, f => f.Random.Number(2000, 2021).ToString());
            return carFaker.Generate();
        }

    }
}
