using Entities.Model.DTOs;

namespace ApartmentDAL
{
    public interface IApartmentService
    {
        Task<IEnumerable<ApartmentDTO>> GetAllAsync();
        Task<ApartmentDTO> GetByIdAsync(int id);
        Task AddAsync(ApartmentDTO dto);
        Task UpdateAsync(int id, ApartmentDTO dto);
        Task DeleteAsync(int id);
        Task<IEnumerable<ApartmentDTO>> SearchApartments(string location, decimal minPrice, decimal maxPrice, string[] amenities);
        Task<IEnumerable<BookingDTO>> GetBookingsByApartmentId(int apartmentId);

    }

}
