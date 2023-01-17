using System.Collections.Generic;

namespace VacationRental.Common.Entities
{
    public class RentalEntity
    {
        public int Id { get; set; }
        public int Units { get; set; }
        public int PreparationTimeInDays { get; set; }
        public List<BookingEntity> Bookings { get; set; }

        public void AddBooking(BookingEntity booking)
        {
            Bookings.Add(booking);
        }

        public void Update(int units, int preparationTimeInDays)
        {
            Units = units;
            PreparationTimeInDays = preparationTimeInDays;
        }
    }
}