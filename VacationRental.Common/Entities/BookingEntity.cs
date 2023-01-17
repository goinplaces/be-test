using System;

namespace VacationRental.Common.Entities
{
    public class BookingEntity
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }
        public int Unit { get; set; }
        
        public int RentalId { get; set; }
        public RentalEntity Rental { get; set; }
    }
}