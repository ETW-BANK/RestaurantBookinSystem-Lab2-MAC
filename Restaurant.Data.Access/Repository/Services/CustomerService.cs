using Azure;
using Restaurant.Data.Access.Data;
using Restaurant.Data.Access.Repository.IRepository;
using Restaurant.Data.Access.Repository.Services.IServices;
using Restaurant.Models;
using Restaurant.Models.DTOs;
using Restaurant.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Restaurant.Data.Access.Repository.Services
{
    public class CustomerService : ICustomerService

    {
        private readonly ICustomerRepository _customerRepo;



        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepo = customerRepository;
        }

        public async Task<ServiceResponse<string>> AddItemAsync(CustomerDto item)
        {

            var response = new ServiceResponse<string>();


            try
            {
                var customer = new Customer
                {
                    FirstName = item.FirstName,
                    LasttName = item.LasttName,
                    Email = item.Email,
                    Phone = item.Phone
                };

                await _customerRepo.AddItemAsync(customer);
                await _customerRepo.SaveAsync();
                response.Success = true;
                response.Message = Messages.CustomerSucces;
            }
            catch
            {
                response.Success = false;
                response.Message = Messages.CustomerFailed;
            }


            return response;


        }


        public async Task<ServiceResponse<IEnumerable<CustomerDto>>> GetAllAsync()
        {
            var response = new ServiceResponse<IEnumerable<CustomerDto>>();

            try
            {
                var customers = await _customerRepo.GetAllAsync();

                if (customers != null)
                {
                    var customerList = customers.Select(c => new CustomerDto
                    {
                        Id = c.Id,
                        FirstName = c.FirstName,
                        LasttName = c.LasttName,
                        Email = c.Email,
                        Phone = c.Phone
                    }).ToList();


                    response.Data = customerList;
                    response.Success = true;
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
                response.Data = Enumerable.Empty<CustomerDto>();
            }

            return response;
        }


        public async Task<ServiceResponse<CustomerDto>> GetSingleAsync(int id)
        {
            var response = new ServiceResponse<CustomerDto>();

            try
            {
                var customer = await _customerRepo.GetSingleAsync(id);

                if (customer != null)
                {
                    var customerDto = new CustomerDto
                    {
                        Id = customer.Id,
                        FirstName = customer.FirstName,
                        LasttName = customer.LasttName,
                        Email = customer.Email,
                        Phone = customer.Phone
                    };

                    response.Success = true;
                    response.Data = customerDto;
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

        

            return  response;
        }

        public async Task<ServiceResponse<bool>> RemoveAsync(int id)
        {
            var response = new ServiceResponse<bool>();
            {
                try
                {
                    Customer customeer = await _customerRepo.GetSingleAsync(id);

                    if (customeer != null)
                    {
                        await _customerRepo.RemoveAsync(id);
                        await _customerRepo.SaveAsync();

                        response.Data = true;
                        response.Success = true;
                        response.Message = Messages.DataDelete;
                    }
                    else
                    {
                        response.Data = false;
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
        }



        public async Task<ServiceResponse<bool>> UpdateCustomerAsync(CustomerDto customer)
        {
            var response = new ServiceResponse<bool>();

            try
            {

                var existingCustomer = await _customerRepo.GetSingleAsync(customer.Id);

                 
                if (existingCustomer != null)
                {
                    existingCustomer.FirstName = customer.FirstName;
                    existingCustomer.LasttName = customer.LasttName;
                    existingCustomer.Email = customer.Email;
                    existingCustomer.Phone = customer.Phone;

                    await _customerRepo.UpdateCustomerAsync(existingCustomer);


                    response.Data = true; 
                    response.Success = true;
                    response.Message = Messages.CustomerUpdateSucces; 
                }
                else
                {
                    response.Data = false; 
                    response.Success = false;
                    response.Message = Messages.NoData; 
                }
            }
            catch
            {
                response.Data = false;
                response.Success = false;
                response.Message = Messages.CustomerUpdateFailed; 
            }

            return response;
        }
    }
}
    
    

