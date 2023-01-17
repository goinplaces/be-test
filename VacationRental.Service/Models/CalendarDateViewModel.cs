using System;
using System.Collections.Generic;

namespace VacationRental.Service.Models
{
    public class CalendarDateViewModel
    {
        public DateTime Date { get; set; }
        public List<CalendarBookingViewModel> Bookings { get; set; }
        public List<PreparationViewModel> PreparationTimes { get; set; }
    }
}
