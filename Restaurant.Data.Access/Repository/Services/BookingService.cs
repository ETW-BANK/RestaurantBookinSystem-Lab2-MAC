using Restaurant.Data.Access.Repository.IRepository;
using Restaurant.Data.Access.Repository.Services.IServices;
using Restaurant.Models.DTOs;
using Restaurant.Models;
using Restaurant.Utility;
using System.Linq.Expressions;
using Restaurant.Data.Access.Repository;

public class BookingService : IBookingService
{
    private readonly IBookngRepository _bookingRepo;
    private readonly ITableRepository _tableRepo;
    private readonly ICustomerRepository _customerRepo;


    public BookingService(IBookngRepository bookingRepo, ITableRepository tableRepo, ICustomerRepository customerRepo)
    {
        _bookingRepo = bookingRepo;
        _tableRepo = tableRepo;
        _customerRepo = customerRepo;

    }

    public async Task<ServiceResponse<string>> AddItemAsync(BookingDto item)
    {
        var response = new ServiceResponse<string>();

        try
        {

            var customer = await _customerRepo.GetSingleAsync(item.Id);
            if (customer == null)
            {
                customer = new Customer
                {
                    FirstName = item.Customer.FirstName,
                    LasttName = item.Customer.LasttName,
                    Email = item.Customer.LasttName,
                    Phone = item.Customer.LasttName,
                };
                await _customerRepo.AddItemAsync(customer);
                await _customerRepo.SaveAsync();
            }

            var availableTables = await _tableRepo.GetAllAsync();
            var suitableTable = availableTables
                .Where(t => t.NumberOfSeats >= item.NumberOfGuests && t.isAvialable)
                .OrderBy(t => t.NumberOfSeats)
                .FirstOrDefault();

            if (suitableTable == null)
            {
                response.Success = false;
                response.Message = "No suitable table available for the number of guests.";
                return response;
            }


            var newBooking = new Booking
            {
                BookingDate = item.BookingDate,
                NumberOfGuests = item.NumberOfGuests,
                Tables = suitableTable,
                Customer = customer
            };


            await _bookingRepo.AddItemAsync(newBooking);
            await _bookingRepo.SaveAsync();

            suitableTable.isAvialable = false;
            await _tableRepo.UpdateTableAsync(suitableTable);
            await _tableRepo.SaveAsync();

            response.Data = newBooking.Id.ToString();
            response.Success = true;
            response.Message = Messages.BookingSucces;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
        }

        return response;
    }





    public async Task<ServiceResponse<IEnumerable<BookingDto>>> GetAllAsync()
    {
        var bookings = await _bookingRepo.GetAllAsync(
            b => b.Customer,
            b => b.Tables

        );

        var bookingDtos = bookings.Select(b => new BookingDto
        {
            Id = b.Id,
            BookingDate = b.BookingDate,
            NumberOfGuests = b.NumberOfGuests,
            CustomerId = b.CustomerId,
            TablesId = b.TablesId,
            Customer = b.Customer != null ? new CustomerDto
            {
                Id = b.Customer.Id,
                FirstName = b.Customer.FirstName,
                LasttName = b.Customer.LasttName,
                Email = b.Customer.Email,
                Phone = b.Customer.Phone
            } : null,

            Tables = b.Tables != null ? new TablesDto
            {
                Id = b.Tables.Id,
                TableNumber = b.Tables.TableNumber,
                NumberOfSeats = b.Tables.NumberOfSeats,
                isAvialable = b.Tables.isAvialable
            } : null
        }).ToList();

        return new ServiceResponse<IEnumerable<BookingDto>>
        {
            Data = bookingDtos,
            Success = true,
            Message = null
        };
    }






    public async Task<ServiceResponse<BookingDto>> GetSingleAsync(int id, params Expression<Func<Booking, object>>[] includes)
    {
        var response = new ServiceResponse<BookingDto>();

        try
        {

            var singleBooking = await _bookingRepo.GetSingleAsync(id, b => b.Customer, b => b.Tables/*, b => b.FoodMenu*/);

            if (singleBooking != null)
            {



                response.Data = new BookingDto
                {
                    Id = singleBooking.Id,
                    BookingDate = singleBooking.BookingDate,
                    TablesId = singleBooking.TablesId,
                    CustomerId = singleBooking.CustomerId,
                    NumberOfGuests = singleBooking.NumberOfGuests,

                    Customer = singleBooking.Customer != null ? new CustomerDto
                    {
                        Id = singleBooking.Customer.Id,
                        FirstName = singleBooking.Customer.FirstName,
                        LasttName = singleBooking.Customer.LasttName,
                        Email = singleBooking.Customer.Email,
                        Phone = singleBooking.Customer.Phone
                    } : null,


                    Tables = singleBooking.Tables != null ? new TablesDto
                    {
                        Id = singleBooking.Tables.Id,
                        TableNumber = singleBooking.Tables.TableNumber,
                        NumberOfSeats = singleBooking.Tables.NumberOfSeats,
                        isAvialable = singleBooking.Tables.isAvialable
                    } : null,


                };

                response.Success = true;
                response.Message = Messages.BookingRetrival;
            }
            else
            {
                response.Success = false;
                response.Message = Messages.NoData;
            }
        }
        catch
        {
            response.Success = false;
            response.Message = Messages.NoData;
        }

        return response;
    }



    public async Task<ServiceResponse<bool>> RemoveAsync(int id)
    {
        var response = new ServiceResponse<bool>();

        try
        {
            var bookingToDelete = await _bookingRepo.GetSingleAsync(id);
            if (bookingToDelete != null)
            {

                var table = await _tableRepo.GetSingleAsync(bookingToDelete.TablesId);

                if (table != null)
                {
                    table.isAvialable = true;
                }


                await _bookingRepo.RemoveAsync(id);

                await _bookingRepo.SaveAsync();

                response.Data = true;
                response.Success = true;
                response.Message = Messages.DataDelete;
            }
            else
            {
                response.Success = false;
                response.Message = Messages.NoData;
            }
        }
        catch
        {
            response.Data = false;
            response.Success = false;
            response.Message = Messages.DFailDelete;
        }

        return response;
    }




    public async Task<ServiceResponse<bool>> UpdateBookingAsync(BookingDto bookingDto)
    {
        var response = new ServiceResponse<bool>();

        try
        {
            // Fetch the existing booking
            var existingBooking = await _bookingRepo.GetSingleAsync(bookingDto.Id);

            if (existingBooking == null)
            {
                response.Success = false;
                response.Message = Messages.NoData;
                return response;
            }

            // Fetch the table
            var table = await _tableRepo.GetSingleAsync(bookingDto.TablesId);
            if (table == null)
            {
                response.Success = false;
                response.Message = Messages.BookingTableNotFound;
                return response;
            }

            // Fetch the customer
            var customer = await _customerRepo.GetSingleAsync(bookingDto.CustomerId);
            if (customer == null)
            {
                response.Success = false;
                response.Message = Messages.CustomerUpdateFailed;
                return response;
            }

            // Check if the number of guests exceeds the table's limit
            if (bookingDto.NumberOfGuests > table.NumberOfSeats)
            {
                response.Success = false;
                response.Message = Messages.BookingTableLimit;
                return response;
            }

            // Update the existing booking properties
            existingBooking.NumberOfGuests = bookingDto.NumberOfGuests;
            existingBooking.BookingDate = bookingDto.BookingDate;

            // Update table and customer relationships
            existingBooking.TablesId = bookingDto.TablesId;
            existingBooking.CustomerId = bookingDto.CustomerId;

            // Optionally set navigation properties (if needed)
            existingBooking.Tables = table;
            existingBooking.Customer = customer;

            // Update the booking in the repository
            await _bookingRepo.UpdateBookingAsync(existingBooking);

            // Save changes to the database
            await _bookingRepo.SaveAsync();

            response.Data = true;
            response.Success = true;
            response.Message = Messages.BookingUpdateSucces;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
        }

        return response;
    }
}