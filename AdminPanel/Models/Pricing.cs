using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Models
{
    class Pricing
    {
        public float MonthlyIncome { get; set; }

        public float PricePerKm { get; set; }

        public float DailyIncome { get; set; }

        public float Interest { get; set; }

        public DateTime LastWithDraw { get; set; }

        public Pricing()
        {
            PricePerKm = 0.7f;
            LastWithDraw = new DateTime(2021, 11, 12);
        }
    }
}
