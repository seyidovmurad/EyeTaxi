using UserPanel.Services;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminPanel.Services;

namespace AdminPanel.Models
{
    public class Driver
    {
        public int Id { get; set; }

        public int RatingSum { get; set; }

        public int RatingCount { get; set; }
        
        public float Balance { get; set; }

        public string Rating { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string PhoneNumber { get; set; }

        public Location LastLocation { get; set; }

        public Car Car { get; set; }

        public Driver(float balance, string rating, string name, string surname, string phoneNumber, Location lastLocation, Car car)
        {
            Balance = balance;
            Rating = rating;
            Name = name;
            Surname = surname;
            PhoneNumber = phoneNumber;
            LastLocation = lastLocation;
            Car = car;
        }

        public Driver()
        {
            LastLocation = new Location();
            Car = new Car();
            Rating = "5";

        }

        public bool isEmpty()
        {
            return string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(Surname) && string.IsNullOrEmpty(PhoneNumber) && Car.IsEmpty();
        }

        public bool IsWhiteSpace()
        {
            return string.IsNullOrWhiteSpace(Name) && string.IsNullOrWhiteSpace(Surname) && string.IsNullOrWhiteSpace(PhoneNumber) && Car.IsWhiteSpace();
        }
    }
}
