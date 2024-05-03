using Entities.Model;

namespace ApartmentDAL
{
    public interface IApartmentRepository
    {
        Task<IEnumerable<Apartment>> GetAllAsync();
        Task<Apartment> GetByIdAsync(int id);
        Task AddAsync(Apartment entity);
        Task UpdateAsync(Apartment entity);
        Task DeleteAsync(int id);
        Task<IEnumerable<Apartment>> SearchApartments(string location, decimal minPrice, decimal maxPrice, string[] amenities);

        Task<IEnumerable<Booking>> GetBookingsByApartmentId(int apartmentId);
    }

}
