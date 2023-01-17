using System;

namespace VacationRental.Common.Models
{
    public class BookingBindingModel
    {
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }
    }
}