using System.Collections.Generic;
using System.Threading.Tasks;
using VacationRental.Common.Models;

namespace VacationRental.Service.Interfaces
{
    public interface IBookingService
    {
        Task<BookingViewModel> GetBooking(int bookingId);
        Task<ResourceIdViewModel> AddBooking(BookingBindingModel model);
        IEnumerable<OverlappedBookingViewModel> GetOverlaps(RentalViewModel newRental);
    }
}