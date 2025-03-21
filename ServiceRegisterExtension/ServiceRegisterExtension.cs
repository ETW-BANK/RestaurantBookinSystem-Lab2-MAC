﻿using Microsoft.Extensions.DependencyInjection;
using Restaurant.Data.Access.DbInisializer;
using Restaurant.Data.Access.Repository;
using Restaurant.Data.Access.Repository.IRepository;
using Restaurant.Data.Access.Repository.Services;
using Restaurant.Data.Access.Repository.Services.IServices;
using Restaurant.Services;
using RestaurantServices.Services;
using RestaurantServices.Services.IServices;



namespace ServiceRegisterExtension
{
    public class ServiceRegisterExtension : IServicesRegisterExtension
    {

        public ServiceRegisterExtension()
        {
            
        }
        public void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITableService, TableService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IMenuService,MenuService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IDbInitilizer, DbInitializer>();
            services.AddScoped<IUnitOfWork,UnitOfWork>();

        }
    }
}