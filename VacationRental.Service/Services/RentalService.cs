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
    public class RentalService : IRentalService
    {
        private readonly IBookingService _bookingService;
        private readonly IMapper _mapper;
        private readonly VacationRentalDbContext _dbContext;

        public RentalService(IBookingService bookingService, IMapper mapper, VacationRentalDbContext dbContext)
        {
            _bookingService = bookingService;
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<RentalViewModel> Get(int rentalId)
        {
            var rental = await _dbContext.GetRental(rentalId);
            if (rental == null)
            {
                throw new ApplicationException(AppConstants.NoRental);
            }

            return _mapper.Map<RentalViewModel>(rental);
        }

        public ResourceIdViewModel Post(RentalModel model)
        {
            var rental = new RentalEntity
            {
                Units = model.Units,
                PreparationTimeInDays = model.PreparationTimeInDays,
                Bookings = new List<BookingEntity>()
            };

            _dbContext.AddRental(rental);
            _dbContext.SaveChangesAsync();

            return new ResourceIdViewModel
            {
                Id = rental.Id
            };
        }

        public async Task<RentalViewModel> UpdateRental(int rentalId, RentalModel model)
        {
            var existingRental = await _dbContext.GetRental(model.Id);
            if (existingRental == null)
            {
                throw new ApplicationException(AppConstants.NoRental);
            }

            var newRental = new RentalViewModel(rentalId, model);
            var overlappedBookings = _bookingService.GetOverlaps(newRental).FirstOrDefault();
            if (overlappedBookings != null)
            {
                throw new ApplicationException(AppConstants.OverlappedBookings);
            }
            
            existingRental.Update(model.Units, model.PreparationTimeInDays);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<RentalViewModel>(existingRental);
        }
    }
}