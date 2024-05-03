using AutoMapper;
using Entities.Model;
using Entities.Model.DTOs;

namespace Entities.Model.DTOs
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User mappings
            CreateMap<User, UserDTO>(); // Map User entity to UserDTO
            CreateMap<UserDTO, User>(); // Map UserDTO to User entity

            // Booking mappings
            CreateMap<Booking, BookingDTO>(); // Map Booking entity to BookingDTO
            CreateMap<BookingDTO, Booking>(); // Map BookingDTO to Booking entity

            // Add more mappings as needed
            CreateMap<Apartment, ApartmentDTO>(); // Map Apartment entity to ApartmentDTO
            CreateMap<ApartmentDTO, Apartment>(); // Map ApartmentDTO to Apartment entity
        }
    }
}
