using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Service.Interfaces;
using VacationRental.Common.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IRentalService _rentalService;

        public RentalsController(IRentalService rentalService)
        {
            _rentalService = rentalService;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public async Task<RentalViewModel> Get(int rentalId)
        {
            return await _rentalService.Get(rentalId);
        }

        [HttpPost]
        public ResourceIdViewModel Post(RentalModel model)
        {
            return _rentalService.Post(model);
        }
        
        [HttpPut("{rentalId:int}")]
        public async Task<RentalViewModel> Put(int rentalId, [FromBody]RentalModel model)
        {
            model.Id = rentalId;
            return await _rentalService.UpdateRental(rentalId, model);
        }
    }
}
