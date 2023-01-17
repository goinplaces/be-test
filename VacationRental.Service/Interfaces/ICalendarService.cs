using System;
using System.Threading.Tasks;
using VacationRental.Common.Models;

namespace VacationRental.Service.Interfaces
{
    public interface ICalendarService
    {
        Task<CalendarViewModel> Get(int rentalId, DateTime start, int nights);
    }
}