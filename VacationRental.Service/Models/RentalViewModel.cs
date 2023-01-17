using VacationRental.Common.Entities;

namespace VacationRental.Service.Models
{
    public class RentalViewModel
    {
        public RentalViewModel(int id, RentalModel model)
        {
            Id = id;
            Units = model.Units;
            PreparationTimeInDays = model.PreparationTimeInDays;
        }

        public RentalViewModel()
        {
        }

        public RentalViewModel(RentalEntity rental)
        {
            Id = rental.Id;
            Units = rental.Units;
            PreparationTimeInDays = rental.PreparationTimeInDays;
        }

        public int Id { get; set; }
        public int Units { get; set; }
        public int PreparationTimeInDays { get; set; }
    }
}
