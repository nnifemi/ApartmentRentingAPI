using Entities.Model;
using Microsoft.EntityFrameworkCore;

namespace ApartmentDAL
{
    public class ApartmentRepository : IApartmentRepository
    {
        private readonly RentingDbContext _context;

        public ApartmentRepository(RentingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Apartment>> GetAllAsync()
        {
            return await _context.Apartments.ToListAsync();
        }

        public async Task<Apartment> GetByIdAsync(int id)
        {
            return await _context.Apartments.FindAsync(id);
        }

        public async Task AddAsync(Apartment entity)
        {
            await _context.Apartments.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Apartment entity)
        {
            _context.Apartments.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var apartment = await GetByIdAsync(id);
            if (apartment != null)
            {
                _context.Apartments.Remove(apartment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Apartment>> SearchApartments(string location, decimal minPrice, decimal maxPrice, string[] amenities)
        {
            var query = _context.Apartments.AsQueryable();

            if (!string.IsNullOrEmpty(location))
            {
                query = query.Where(a => a.Location.ToLower().Contains(location.ToLower()));
            }

            if (minPrice > 0)
            {
                query = query.Where(a => a.Price >= minPrice);
            }

            if (maxPrice > 0)
            {
                query = query.Where(a => a.Price <= maxPrice);
            }

            if (amenities != null && amenities.Any())
            {
                foreach (var amenity in amenities)
                {
                    query = query.Where(a => a.Amenities.ToLower().Contains(amenity.ToLower()));
                }
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingsByApartmentId(int apartmentId)
        {
            return await _context.Bookings.Where(b => b.ApartmentID == apartmentId).ToListAsync();
        }
    }

}
