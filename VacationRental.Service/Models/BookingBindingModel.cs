﻿using System;

namespace VacationRental.Service.Models
{
    public class BookingBindingModel
    {
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }
    }
}