﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Models
{
    public class Pricing
    {
        public float PricePerKm { get; set; }

        public Pricing()
        {
            PricePerKm = 0.7f;
        }
    }
}
