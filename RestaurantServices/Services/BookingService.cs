﻿
using Restaurant.Data.Access.Repository.IRepository;
using Restaurant.Models;
using RestaurantServices.Services.IServices;
using RestaurantViewModels;

namespace RestaurantServices.Services
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;


        public BookingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;


        }


        public void CreateBooking(BookingVM bookingVM)
        {
          
            if (!TimeSpan.TryParse(bookingVM.BookingTime, out var bookingTime))
            {
                throw new ArgumentException("Invalid booking time format.");
            }

           
            var table = _unitOfWork.TableRepository.GetFirstOrDefault(t => t.Id == bookingVM.TableId);
            if (table == null)
            {
                throw new ArgumentException($"Table with ID {bookingVM.TableId} does not exist.");
            }

           
            var booking = new Booking
            {
                BookingDate = bookingVM.BookingDate,
                BookingTime = bookingTime,
                NumberOfGuests = bookingVM.NumberOfGuests,
                ApplicationUserId = bookingVM.ApplicationUserId,
                TableId = bookingVM.TableId,
                

            };
            _unitOfWork.BookingRepository.Add(booking);
            _unitOfWork.Save();
        }


        public async Task<IEnumerable<BookingVM>> GetBookingsAsync()
        {
           
            var bookings = _unitOfWork.BookingRepository
                .GetAll(
                    includeProperties: "ApplicationUser,Tables"  
                )
                .ToList();

          
            var bookingsList = bookings.Select(b => new BookingVM
            {
                BookingId = b.Id,
                BookingDate = b.BookingDate,
                BookingTime = b.BookingTime.ToString(@"hh\:mm"),  
                NumberOfGuests = b.NumberOfGuests,
                TableId = b.Tables.Id,
                TableNumber=b.Tables.TableNumber,
                ApplicationUserId= b.ApplicationUserId,
                Name=b.ApplicationUser.Name,
                Email=b.ApplicationUser.Email,  
                Phone=b.ApplicationUser.PhoneNumber,
               
                
            }).ToList();

            return bookingsList;
        }




    }
}