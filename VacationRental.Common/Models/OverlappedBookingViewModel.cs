using System.Collections.Generic;

namespace VacationRental.Common.Models
{
    public sealed class OverlappedBookingViewModel
    {
        public IReadOnlyCollection<BookingViewModel> OverlappedBookings { get; set; }
    }
}
