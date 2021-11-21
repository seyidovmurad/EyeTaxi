using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Models
{
    public class Car
    {
        public string Model { get; set; }

        public string Vendor { get; set; }

        public string Color { get; set; }

        public string Year { get; set; }

        public Car(string model, string vendor, string color, string year)
        {
            Model = model;
            Vendor = vendor;
            Color = color;
            Year = year;
        }

        public Car()
        {
        }
        
        public override string ToString()
        {
            return $"{Model} {Vendor}";
        }

        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(Model) && string.IsNullOrEmpty(Vendor) && string.IsNullOrEmpty(Color) && string.IsNullOrEmpty(Year);
        }

        public bool IsWhiteSpace()
        {
            return !(string.IsNullOrWhiteSpace(Model) && string.IsNullOrWhiteSpace(Vendor) && string.IsNullOrWhiteSpace(Color) && string.IsNullOrWhiteSpace(Year));
        }
    }
}
