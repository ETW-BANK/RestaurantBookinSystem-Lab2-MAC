
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


        public async Task CreateBookingAsync(BookingVM bookingVM)
        {
           
            if (!TimeSpan.TryParse(bookingVM.BookingTime, out var bookingTime))
            {
                throw new ArgumentException("Invalid booking time format. Expected format: HH:mm.");
            }

            if (bookingVM.NumberOfGuests <= 0)
            {
                throw new ArgumentException("Number of guests must be greater than zero.");
            }

           
            var availableTables = _unitOfWork.TableRepository
                .GetAll(t => t.IsAvailable && t.NumberOfSeats >= bookingVM.NumberOfGuests)
                .ToList();

       
            if (availableTables.Count==0)
            {
                throw new InvalidOperationException("No available tables at this time.");
            }
           
            var availableTable = availableTables.First();

      
            if (bookingVM.NumberOfGuests > availableTable.NumberOfSeats)
            {
                throw new ArgumentException("Number of guests exceeds the seating capacity of the selected table.");
            }


            var booking = new Booking
            {
                BookingDate = DateOnly.FromDateTime(bookingVM.BookingDate),
                ApplicationUser = new ApplicationUser { Name = bookingVM.Name, PhoneNumber = bookingVM.Phone, Email = bookingVM.Email },
                BookingTime = bookingTime,
                NumberOfGuests = bookingVM.NumberOfGuests,
                ApplicationUserId = bookingVM.ApplicationUserId,
                TableId = availableTable.Id,
                BookingStatus = BookingStatus.Active,
             
            };

       
            _unitOfWork.BookingRepository.Add(booking);
            await _unitOfWork.SaveAsync();
        }


        public async Task<IEnumerable<BookingVM>> GetBookingsAsync()
        {
            var bookings =  _unitOfWork.BookingRepository
                .GetAll(includeProperties: "ApplicationUser,Tables");

            var bookingsList = bookings.Select(b => new BookingVM
            {
                BookingId = b.Id,
                BookingDate = b.BookingDate.ToDateTime(TimeOnly.MinValue),
                BookingTime = b.BookingTime.ToString(@"hh\:mm"),
                NumberOfGuests = b.NumberOfGuests,
                TableNumber = b.Tables.TableNumber,
                ApplicationUserId = b.ApplicationUserId,
                Name = b.ApplicationUser.Name,
                Email = b.ApplicationUser.Email,
                Phone = b.ApplicationUser.PhoneNumber,
                BookingStatus = b.BookingStatus,
            }).ToList();

            return bookingsList;
        }


        public BookingVM GetSingle(int id)
        {
            var booking = _unitOfWork.BookingRepository.GetFirstOrDefault(
    x => x.Id == id, includeProperties: "ApplicationUser");

            if (booking == null)
            {
                return null; 
            }

            var bookingVM = new BookingVM
            {
                BookingId = booking.Id,
                BookingDate = booking.BookingDate.ToDateTime(TimeOnly.MinValue), 
                BookingTime = booking.BookingTime.ToString(), 
                TableNumber = booking.TableId, 
                NumberOfGuests = booking.NumberOfGuests,
                BookingStatus = booking.BookingStatus,
                ApplicationUserId = booking.ApplicationUserId,
                Name=booking.ApplicationUser.Name,
                Phone=booking.ApplicationUser.PhoneNumber,
                Email=booking.ApplicationUser.Email
                
            };

         
            if (booking.ApplicationUser != null)
            {
                bookingVM.applicationUser = new ApplicationUser
                {
                    Name = booking.ApplicationUser.Name,
                    PhoneNumber = booking.ApplicationUser.PhoneNumber,
                    Email=booking.ApplicationUser.Email
                    
                };
            }

            return bookingVM;
        }


        public async Task<Booking> DeleteBooking(Booking booking)
        {
            if (booking == null)
            {
                throw new ArgumentNullException(nameof(booking), "Booking cannot be null.");
            }

            var existingBooking = _unitOfWork.BookingRepository.GetFirstOrDefault(x => x.Id == booking.Id);
            if (existingBooking == null)
            {
                throw new InvalidOperationException("Booking not found.");
            }

          
            var table = _unitOfWork.TableRepository.GetFirstOrDefault(x => x.Id == existingBooking.TableId);
            if (table != null)
            {
                table.IsAvailable = true;
                _unitOfWork.TableRepository.UpdateTable(table); 
            }

        
            _unitOfWork.BookingRepository.Remove(existingBooking);

        
            await _unitOfWork.SaveAsync();

            return existingBooking;
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
            _unitOfWork.SaveAsync();

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

        public void Update(BookingVM booking)
        {
            var existingbooking= _unitOfWork.BookingRepository.GetFirstOrDefault(x => x.Id == booking.BookingId);

            if (existingbooking == null)
            {
                throw new Exception("Booking not found");
            }


           existingbooking.Id = booking.BookingId;
            existingbooking.BookingDate = DateOnly.FromDateTime(booking.BookingDate);
            existingbooking.BookingTime = TimeSpan.Parse(booking.BookingTime);
            existingbooking.NumberOfGuests = booking.NumberOfGuests;
            existingbooking.ApplicationUserId = booking.ApplicationUserId;
            existingbooking.BookingStatus = booking.BookingStatus;


            _unitOfWork.BookingRepository.UpdateBooking(existingbooking);
            _unitOfWork.SaveAsync();
        }
    }
}