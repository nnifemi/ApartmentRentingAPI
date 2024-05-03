using ApartmentDAL;
using AutoMapper;
using Entities.Model.DTOs;
using Entities.Model;

namespace ApartmentBLL
{
    public class ApartmentService : IApartmentService
    {
        private readonly IApartmentRepository _apartmentRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;

        public ApartmentService(IApartmentRepository apartmentRepository, IBookingRepository bookingRepository, IMapper mapper)
        {
            _apartmentRepository = apartmentRepository;
            _bookingRepository = bookingRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ApartmentDTO>> GetAllAsync()
        {
            var apartments = await _apartmentRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ApartmentDTO>>(apartments);
        }

        public async Task<ApartmentDTO> GetByIdAsync(int id)
        {
            var apartment = await _apartmentRepository.GetByIdAsync(id);
            return _mapper.Map<ApartmentDTO>(apartment);
        }

        public async Task AddAsync(ApartmentDTO dto)
        {
            var apartment = _mapper.Map<Apartment>(dto);
            await _apartmentRepository.AddAsync(apartment);
        }

        public async Task UpdateAsync(int id, ApartmentDTO dto)
        {
            var existingApartment = await _apartmentRepository.GetByIdAsync(id);
            if (existingApartment == null)
            {
                throw new Exception("Apartment not found");
            }

            _mapper.Map(dto, existingApartment);
            await _apartmentRepository.UpdateAsync(existingApartment);
        }

        public async Task DeleteAsync(int id)
        {
            await _apartmentRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<ApartmentDTO>> SearchApartments(string location, decimal minPrice, decimal maxPrice, string[] amenities)
        {
            var apartments = await _apartmentRepository.SearchApartments(location, minPrice, maxPrice, amenities);
            return _mapper.Map<IEnumerable<ApartmentDTO>>(apartments);
        }

        public async Task<IEnumerable<BookingDTO>> GetBookingsByApartmentId(int apartmentId)
        {
            var bookings = await _apartmentRepository.GetBookingsByApartmentId(apartmentId);
            return _mapper.Map<IEnumerable<BookingDTO>>(bookings);
        }
    }

}
