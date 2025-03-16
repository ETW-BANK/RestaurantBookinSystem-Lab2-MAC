
using FluentAssertions;
using Moq;
using Restaurant.Data.Access.Repository.IRepository;
using Restaurant.Models;
using RestaurantServices.Services;
using RestaurantViewModels;
using System.Linq.Expressions;

public class BookingServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly BookingService _bookingService;

    public BookingServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _bookingService = new BookingService(_mockUnitOfWork.Object);
    }
    [Fact]
    public async Task CreateBookingAsync_ValidBooking_CreatesBooking()
    {
        // Arrange
        var bookingVM = new BookingVM
        {
            BookingDate = DateTime.Now,
            BookingTime = "12:00",
            NumberOfGuests = 4,
            ApplicationUserId = "user123",
            Name = "John Doe",
            Phone = "1234567890",
            Email = "john.doe@example.com"
        };

        var availableTable = new Tables { Id = 1, IsAvailable = true, NumberOfSeats = 4 };

        _mockUnitOfWork.Setup(uow => uow.TableRepository.GetFirstOrDefault(
                It.IsAny<Expression<Func<Tables, bool>>>(), // Mock the filter
                null, // Mock includeProperties (null in this case)
                It.IsAny<bool>() // Mock the tracked flag
            ))
            .Returns((Expression<Func<Tables, bool>> filter, string includeProperties, bool tracked) =>
            {
                // Simulate the behavior of GetFirstOrDefault
                var query = new List<Tables> { availableTable }.AsQueryable().Where(filter);
                return query.FirstOrDefault();
            });

        // Act
        await _bookingService.CreateBookingAsync(bookingVM);

        // Assert
        _mockUnitOfWork.Verify(uow => uow.BookingRepository.Add(It.IsAny<Booking>()), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.SaveAsync(), Times.Once);
    }

    //[Fact]
    //public async Task CreateBookingAsync_NoAvailableTables_ThrowsException()
    //{
    //    // Arrange
    //    var bookingVM = new BookingVM
    //    {
    //        BookingDate = DateTime.Now,
    //        BookingTime = "12:00",
    //        NumberOfGuests = 4,
    //        ApplicationUserId = "user123",
    //        Name = "John Doe",
    //        Phone = "1234567890",
    //        Email = "john.doe@example.com"
    //    };

    //    // Mock the GetAll method to return an empty list (no available tables)
    //    _mockUnitOfWork.Setup(uow => uow.TableRepository.GetAll(
    //            It.IsAny<Expression<Func<Tables, bool>>>(), // Mock the filter
    //            null, // Mock includeProperties (null in this case)
    //            It.IsAny<bool>() // Mock the tracked flag
    //        ))
    //        .Returns(new List<Tables>().AsQueryable()); // Return an empty queryable list

    //    // Act
    //    Func<Task> act = async () => await _bookingService.CreateBookingAsync(bookingVM);

    //    // Assert
    //    await act.Should().ThrowAsync<InvalidOperationException>()
    //        .WithMessage("No available tables at this time.");
    //}
}