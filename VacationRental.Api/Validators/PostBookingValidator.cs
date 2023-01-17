using FluentValidation;
using VacationRental.Common.Models;

namespace VacationRental.Api{
    public class PostBookingValidator : AbstractValidator<BookingBindingModel>{
        public PostBookingValidator(){
            RuleFor(r=>r.RentalId).GreaterThan(0).WithMessage(m=>AppConstants.NoRentalId);
            RuleFor(r=>r.Nights).GreaterThan(0).WithMessage(m=>AppConstants.InvalidNights);
            RuleFor(r=>r.Start).GreaterThanOrEqualTo(System.DateTime.Today).WithMessage(m=>AppConstants.InvalidStartDate);
        }
    }
}