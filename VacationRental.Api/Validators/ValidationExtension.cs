using System;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace VacationRental.Api{
    public static class ValidationExtension{
        public static async Task ValidateAsync<TRequest>(this TRequest request, IValidator<TRequest> validator){
            if(validator != null){
                var result = await validator.ValidateAsync(request).ConfigureAwait(false);
                if(!result.IsValid){
                    var error = result.Errors.FirstOrDefault();
                   throw new ApplicationException(error.ErrorMessage);
                }
            }
        }
    }
}