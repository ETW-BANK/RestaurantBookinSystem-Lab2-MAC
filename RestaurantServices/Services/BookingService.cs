
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
                BookingStatus = BookingStatus.Active,

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
                BookingStatus=b.BookingStatus,  
               
                
            }).ToList();

            return bookingsList;
        }

        public Booking GetSinle(int bookingId)
        {
            var booking = _unitOfWork.BookingRepository.GetFirstOrDefault(x => x.Id == bookingId, includeProperties: "ApplicationUser,Tables");

            if (booking == null)
            {
                throw new Exception("Booking not found.");
            }
            var existingbooking = new BookingVM
            {
                BookingId = bookingId,
                BookingDate = booking.BookingDate,
                BookingTime = booking.BookingTime.ToString(@"hh\:mm"),
                NumberOfGuests = booking.NumberOfGuests,
                TableNumber = booking.Tables.TableNumber,
                ApplicationUserId = booking.ApplicationUserId,
                Name = booking.ApplicationUser.Name,
                Email = booking.ApplicationUser.Email,
                Phone = booking.ApplicationUser.PhoneNumber,
            };

            return booking;

        }

        public Booking DeleteBooking(Booking booking)
        {
            booking = _unitOfWork.BookingRepository.GetFirstOrDefault(x => x.Id == booking.Id);

            if (booking == null)
            {
                throw new Exception("Booking not found.");
            }

            var table = _unitOfWork.TableRepository.GetFirstOrDefault(x => x.Id == booking.TableId);

            if (table != null)
            {
                table.IsAvailable = true;
                _unitOfWork.TableRepository.UpdateTable(table);
            }

            _unitOfWork.BookingRepository.Remove(booking);
            _unitOfWork.Save();

            return booking;
        }
        public Booking CancelBooking(Booking booking)
        {
            booking = _unitOfWork.BookingRepository.GetFirstOrDefault(x => x.Id == booking.Id);

            if (booking == null)
            {
                throw new Exception("Booking not found.");
            }

            var table = _unitOfWork.TableRepository.GetFirstOrDefault(x => x.Id == booking.TableId);


            DateTime currentTime = DateTime.Now;

            if (table != null)
            {
                table.IsAvailable = true;
                _unitOfWork.TableRepository.UpdateTable(table);
            }
          


            booking.BookingStatus = BookingStatus.Cancelled;
            _unitOfWork.BookingRepository.UpdateBooking(booking);
            _unitOfWork.Save();

            return booking;
        }
        public IEnumerable<MyBookingsVM> GetBookingsByUserId(string userId)
        {
            var bookings = _unitOfWork.BookingRepository.GetAll(x => x.ApplicationUserId == userId, includeProperties: "ApplicationUser,Tables");

            if (bookings == null || !bookings.Any())
            {
                return null; 
            }

            
            return bookings.Select(b => new MyBookingsVM
            {
                BookingId = b.Id,
                BookingDate = b.BookingDate,
                BookingTime = b.BookingTime.ToString(@"hh\:mm"), 
                NumberOfGuests = b.NumberOfGuests,
                TableId = b.TableId,
                ApplicationUserId=b.ApplicationUserId,
                TableNumber = b.Tables?.TableNumber ?? 0, 
                Name = b.ApplicationUser?.Name, 
                Phone = b.ApplicationUser?.PhoneNumber,
                Email = b.ApplicationUser?.Email,
                BookingStatus=b.BookingStatus 
                
            }).ToList();
        }
    }
}