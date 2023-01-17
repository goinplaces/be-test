using System.Collections.Generic;

namespace VacationRental.Service.Models
{
    public sealed class OverlappedBookingViewModel
    {
        public IReadOnlyCollection<BookingViewModel> OverlappedBookings { get; set; }
    }
}
