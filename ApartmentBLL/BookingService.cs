using ApartmentDAL;
using AutoMapper;
using Entities.Model;
using Entities.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentBLL
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;

        public BookingService(IBookingRepository bookingRepository, IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookingDTO>> GetAllAsync()
        {
            var bookings = await _bookingRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<BookingDTO>>(bookings);
        }

        public async Task<BookingDTO> GetByIdAsync(int id)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            return _mapper.Map<BookingDTO>(booking);
        }

        public async Task AddAsync(BookingDTO dto)
        {
            var booking = _mapper.Map<Booking>(dto);
            await _bookingRepository.AddAsync(booking);
        }

        public async Task UpdateAsync(int id, BookingDTO dto)
        {
            var existingBooking = await _bookingRepository.GetByIdAsync(id);
            if (existingBooking == null)
            {
                throw new Exception("Booking not found");
            }

            _mapper.Map(dto, existingBooking);
            await _bookingRepository.UpdateAsync(existingBooking);
        }

        public async Task DeleteAsync(int id)
        {
            await _bookingRepository.DeleteAsync(id);
        }
    }
}