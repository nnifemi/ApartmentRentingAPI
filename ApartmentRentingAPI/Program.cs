using ApartmentBLL;
using ApartmentDAL;
using ApartmentRentingAPI.Validators;
using AutoMapper;
using Entities.Model;
using Entities.Model.DTOs;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ApartmentRentingAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureServices((context, services) =>
                {
                    // Configure DbContext with the direct connection string
                    services.AddDbContext<RentingDbContext>(options =>
                        options.UseSqlServer("Server=.;Database=ApartmentRenting;Trusted_Connection=True;MultipleActiveResultSets=true;"));

                    // Configure AutoMapper
                    services.AddAutoMapper(typeof(MappingProfile));

                    // Add validators
                    services.AddSingleton<IValidator<UserDTO>, UserValidator>();
                    services.AddSingleton<IValidator<ApartmentDTO>, ApartmentValidator>();
                    services.AddSingleton<IValidator<BookingDTO>, BookingValidator>();

                    // Register repositories and services
                    services.AddScoped<IBookingRepository, BookingRepository>();
                    services.AddScoped<IBookingService, BookingService>();
                    services.AddScoped<IUserRepository, UserRepository>();
                    services.AddScoped<IUserService, UserService>();
                    services.AddScoped<IApartmentRepository, ApartmentRepository>();
                    services.AddScoped<IApartmentService, ApartmentService>();

                    // Add controllers
                    services.AddControllers();
                });
    }
}
