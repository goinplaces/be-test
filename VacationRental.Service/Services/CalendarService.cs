using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VacationRental.Service.Interfaces;
using VacationRental.Data.EFContext;
using VacationRental.Common.Models;

namespace VacationRental.Service.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly VacationRentalDbContext _dbContext;

        public CalendarService(VacationRentalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CalendarViewModel> Get(int rentalId, DateTime start, int nights)
        {
            var rental = await _dbContext.GetRental(rentalId);
            if (rental == null)
            {
                throw new ApplicationException(AppConstants.NoRental);
            }
            
            var result = new CalendarViewModel
            {
                RentalId = rentalId,
                Dates = new List<CalendarDateViewModel>()
            };
            var rentalBookings = rental.Bookings;
            for (var i = 0; i < nights; i++)
            {
                var date = new CalendarDateViewModel
                {
                    Date = start.Date.AddDays(i),
                    Bookings = new List<CalendarBookingViewModel>(),
                    PreparationTimes = new List<PreparationViewModel>()
                };

                foreach (var booking in rentalBookings)
                {
                    var endBookingDate = booking.Start.AddDays(booking.Nights);
                    if (booking.Start <= date.Date && endBookingDate > date.Date)
                    {
                        date.Bookings.Add(new CalendarBookingViewModel {Id = booking.Id, Unit = booking.Unit});
                    }

                    if (endBookingDate <= date.Date && endBookingDate.AddDays(rental.PreparationTimeInDays) > date.Date)
                    {
                        date.PreparationTimes.Add(new PreparationViewModel {Unit = booking.Unit});
                    }
                }

                result.Dates.Add(date);
            }

            return result;
        }
    }
}