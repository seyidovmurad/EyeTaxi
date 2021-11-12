using AdminPanel.Models;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AdminPanel.Services
{

    class FakeDriverService
    {


        
        public static Driver GenerateDriver()
        {
            var driverFaker = new Faker<Driver>("az")
                .RuleFor(d => d.Name, f => f.Name.FirstName())
                .RuleFor(d => d.Surname, f => f.Name.LastName())
                .RuleFor(d => d.PhoneNumber, f => f.Phone.PhoneNumber())
                .RuleFor(d => d.Balance, f => f.Random.Number(20, 300))
                .RuleFor(d => d.Rating, f => f.Random.Number(2, 5));
            var driver = driverFaker.Generate();
            driver.Car = GenerateCar();
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
