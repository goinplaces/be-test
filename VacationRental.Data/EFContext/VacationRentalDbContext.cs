using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VacationRental.Common.Entities;

namespace VacationRental.Data.EFContext
{
    public sealed class VacationRentalDbContext : DbContext
    {
        public VacationRentalDbContext(DbContextOptions<VacationRentalDbContext> options)
            : base(options) { }

        private DbSet<BookingEntity> Bookings { get; set; }

        private DbSet<RentalEntity> Rentals { get; set; }

        public async Task<BookingEntity> GetBooking(int bookingId)
        {
            var booking = await Bookings.FirstOrDefaultAsync(b => b.Id == bookingId);
            return booking;
        }
        
        public async Task<RentalEntity> GetRental(int rentalId)
        {
            var rental = await Set<RentalEntity>().Include(r => r.Bookings).FirstOrDefaultAsync(r => r.Id == rentalId);
            return rental;
        }

        public void AddRental(RentalEntity rental)
        {
            Rentals.Add(rental);
        }
    }
}