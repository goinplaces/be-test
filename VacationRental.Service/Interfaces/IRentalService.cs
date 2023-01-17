using System.Threading.Tasks;
using VacationRental.Common.Models;

namespace VacationRental.Service.Interfaces
{
    public interface IRentalService
    {
        Task<RentalViewModel> Get(int rentalId);
        ResourceIdViewModel Post(RentalModel model);
        Task<RentalViewModel> UpdateRental(int rentalId, RentalModel model);
    }
}