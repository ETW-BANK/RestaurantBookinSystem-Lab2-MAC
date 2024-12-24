
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


        //public void CreateBooking(BookingVM bookingVM)
        //{

        //    if (!TimeSpan.TryParse(bookingVM.BookingTime, out var bookingTime))
        //    {
        //        throw new ArgumentException("Invalid booking time format.");
        //    }


        //    var table = _unitOfWork.TableRepository.GetFirstOrDefault(t => t.Id == bookingVM.TableId);
        //    if (table == null)
        //    {
        //        throw new ArgumentException($"Table with ID {bookingVM.TableId} does not exist.");
        //    }


        //    var booking = new Booking
        //    {
        //        BookingDate = bookingVM.BookingDate,
        //        BookingTime = bookingTime,
        //        NumberOfGuests = bookingVM.NumberOfGuests,
        //        ApplicationUserId = bookingVM.ApplicationUserId,
        //        TableId = bookingVM.TableId,


        //    };
        //    table.IsAvailable = false;
        //    _unitOfWork.BookingRepository.Add(booking);
        //    _unitOfWork.Save();
        //}


        public void CreateBooking(BookingVM bookingVM)
        {
          
            if (!TimeSpan.TryParse(bookingVM.BookingTime, out var bookingTime))
            {
                throw new ArgumentException("Invalid booking time format.");
            }

            
            var availableTable = _unitOfWork.TableRepository.GetFirstOrDefault(t => t.IsAvailable && t.NumberOfSeats >= bookingVM.NumberOfGuests);
            if (availableTable == null)
            {
                throw new InvalidOperationException("No available tables that match the number of guests.");
            }

        
            if (bookingVM.NumberOfGuests <= 0)
            {
                throw new ArgumentException("Number of guests must be greater than zero.");
            }

            if (bookingVM.NumberOfGuests > availableTable.NumberOfSeats)
            {
                throw new ArgumentException("Number of guests exceeds the seating capacity of the selected table.");
            }

            
            var booking = new Booking
            {
                BookingDate = bookingVM.BookingDate,
                BookingTime = bookingTime,
                NumberOfGuests = bookingVM.NumberOfGuests,
                ApplicationUserId = bookingVM.ApplicationUserId,
                TableId = availableTable.Id,

            };

           
            availableTable.IsAvailable = false;

            _unitOfWork.TableRepository.UpdateTable(availableTable);  
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