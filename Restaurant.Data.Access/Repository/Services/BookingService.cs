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

            // Mark table as not available
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
            //b => b.FoodMenu
        );

        var bookingDtos = bookings.Select(b => new BookingDto
        {
            Id = b.Id,
            BookingDate = b.BookingDate,
            NumberOfGuests = b.NumberOfGuests,
            CustomerId = b.CustomerId,
            TablesId = b.TablesId,
            //FoodMenuId = b.FoodMenu?.Id ?? 0,
            Customer = b.Customer != null ? new CustomerDto
            {
                Id = b.Customer.Id,
                FirstName = b.Customer.FirstName,
                LasttName = b.Customer.LasttName,
                Email = b.Customer.Email,
                Phone = b.Customer.Phone
            } : null,
            //FoodMenu = b.FoodMenu != null ? new FoodMenuDto
            //{
            //    Id = b.FoodMenu.Id,
            //    Title = b.FoodMenu.Title,
            //    price = b.FoodMenu.Price,
            //    IsAvailable = b.FoodMenu.IsAvailable,
            //    ImageUrl = b.FoodMenu.ImageUrl
            //} : null,
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

                    //FoodMenu = singleBooking.FoodMenu != null ? new FoodMenuDto
                    //{
                    //    Id = singleBooking.FoodMenu.Id,
                    //    Title = singleBooking.FoodMenu.Title,
                    //    price = singleBooking.FoodMenu.Price,  
                    //    ImageUrl = singleBooking.FoodMenu.ImageUrl,
                    //    IsAvailable = singleBooking.FoodMenu.IsAvailable
                    //} : null
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
            var bookingtodelete=await _bookingRepo.GetSingleAsync(id);
            if (bookingtodelete != null)
            {
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
            
            var existingBooking = await _bookingRepo.GetSingleAsync(bookingDto.Id);
            var table = await _tableRepo.GetSingleAsync(bookingDto.TablesId);
            var menu = await _customerRepo.GetSingleAsync(bookingDto.Id);
            //var foodmenu = await _fooRepo.GetSingleAsync(bookingDto.FoodMenuId);
            var customer = await _customerRepo.GetSingleAsync(bookingDto.Id);

            if (existingBooking == null)
            {
                response.Success = false;
                response.Message = Messages.NoData; 
                return response;
            }

            if (table == null)
            {
                response.Success = false;
                response.Message = Messages.BookingTableNotFound; 
                return response;
            }

           
            if (bookingDto.NumberOfGuests > table.NumberOfSeats)
            {
                response.Success = false;
                response.Message = Messages.BookingTableLimit;
                return response;
            }

           
            existingBooking.NumberOfGuests = bookingDto.NumberOfGuests;
            existingBooking.BookingDate = bookingDto.BookingDate;
            existingBooking.Tables = table;
            //existingBooking.FoodMenu = foodmenu;
            existingBooking.Customer = customer;


            await _bookingRepo.UpdateBookingAsync(existingBooking);
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
