using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using VacationRental.Service.Interfaces;
using VacationRental.Common.Entities;
using VacationRental.Data.EFContext;
using VacationRental.Common.Models;

namespace VacationRental.Service.Services
{
    public class BookingsService : IBookingService
    {
        private readonly VacationRentalDbContext _dbContext;
        private readonly IMapper _mapper;

        public BookingsService(VacationRentalDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<BookingViewModel> GetBooking(int bookingId)
        {
            var booking = await _dbContext.GetBooking(bookingId);
            if (booking == null)
            {
                throw new ApplicationException(AppConstants.NoBooking);
            }
            return _mapper.Map<BookingViewModel>(booking);
        }

        public async Task<ResourceIdViewModel> AddBooking(BookingBindingModel model)
        {
            var rental = await _dbContext.GetRental(model.RentalId);
            if (rental == null)
            {
                throw new ApplicationException(AppConstants.NoRental);
            }
                var spareUnit = GetSpareUnit(rental, model);
                if (spareUnit == 0)
                throw new ApplicationException(AppConstants.NoAvailability);


            var booking = new BookingEntity
            {
                RentalId = model.RentalId,
                Unit = spareUnit,
                Nights = model.Nights,
                Start = model.Start.Date,
            };
            rental.AddBooking(booking);
            await _dbContext.SaveChangesAsync();

            return new ResourceIdViewModel
            {
                Id = booking.Id
            };
        }
        
        public IEnumerable<OverlappedBookingViewModel> GetOverlaps(RentalViewModel newRental)
        {
            var bookings = _dbContext.GetRental(newRental.Id).Result.Bookings.ToArray();

            for (var checkingBookingIdx = 0; checkingBookingIdx < bookings.Length; checkingBookingIdx++)
            {
                var checkedBooking = bookings[checkingBookingIdx];
                if (checkedBooking.Unit > newRental.Units)
                {
                    yield return new OverlappedBookingViewModel();
                }

                for (var bookedIdx = checkingBookingIdx + 1; bookedIdx < bookings.Length; bookedIdx++)
                {
                    var booked = bookings[bookedIdx];
                    if (booked.Unit == checkedBooking.Unit && IsOverlapping(newRental.PreparationTimeInDays, checkedBooking, booked))
                    {
                        yield return new OverlappedBookingViewModel();
                    }
                }
            }
        }

        private static int GetSpareUnit(RentalEntity rental, BookingBindingModel newBooking)
        {
            var newBookingEntity = new BookingEntity{
                Nights = newBooking.Nights,
                Start = newBooking.Start,
            };
            HashSet<int> overlappedUnits = new HashSet<int>();
            foreach(var existingBooking in rental.Bookings){
                //get possible overlapping unit
                if(IsOverlapping(rental.PreparationTimeInDays,existingBooking,newBookingEntity)){
                    overlappedUnits.Add(existingBooking.Unit);
                }
            }

            if (overlappedUnits.Count < rental.Units)
            {
                for (var unit = 1; unit <= rental.Units; unit++)
                {
                    if (!overlappedUnits.Contains(unit))
                    {
                        return unit;
                    }
                }
            }

            return 0;
        }

        private static bool IsOverlapping(int preparationTimeInDays, BookingEntity existingBooking, BookingEntity newBookingEntity)
        {
            var newBookingEndDate = newBookingEntity.Start.AddDays(newBookingEntity.Nights+preparationTimeInDays);
            var existingBookingEndDate = existingBooking.Start.AddDays(newBookingEntity.Nights+preparationTimeInDays);

            return existingBooking.Start <= newBookingEntity.Start && existingBookingEndDate > newBookingEntity.Start ||
                   existingBooking.Start < newBookingEndDate && existingBookingEndDate >= newBookingEndDate ||
                   existingBooking.Start > newBookingEntity.Start && existingBookingEndDate < newBookingEndDate;
        }

    }
}