using Restaurant.Models;
using Restaurant.Models.DTOs;
using Restaurant.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Data.Access.Repository.Services.IServices
{
  public interface IBookingService
    {

        Task<ServiceResponse<string>> AddItemAsync(BookingDto item);
        Task<ServiceResponse<BookingDto>> GetSingleAsync(int id, params Expression<Func<Booking, object>>[] includes);
        Task<ServiceResponse<IEnumerable<BookingDto>>> GetAllAsync();
       
        Task<ServiceResponse<bool>> RemoveAsync(int id);
        Task<ServiceResponse<bool>> UpdateBookingAsync(BookingDto booking);
    }
}
