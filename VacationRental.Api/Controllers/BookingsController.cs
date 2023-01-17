using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Service.Interfaces;
using VacationRental.Common.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IValidator<BookingBindingModel> _validator;


        public BookingsController(
            IBookingService bookingService, IValidator<BookingBindingModel> validator)
        {
            _bookingService = bookingService;
            _validator = validator;
        }

        [HttpGet("{bookingId:int}")]
        public async Task<BookingViewModel> Get(int bookingId)
        {
            return await _bookingService.GetBooking(bookingId);
        }

        [HttpPost]
        public async Task<ResourceIdViewModel> Post(BookingBindingModel model)
        {
            await model.ValidateAsync(_validator).ConfigureAwait(false);
            return await _bookingService.AddBooking(model);
        }
    }
}
