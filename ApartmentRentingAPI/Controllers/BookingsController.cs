using ApartmentDAL;
using ApartmentRentingAPI.Validators;
using AutoMapper;
using Entities.Model;
using Entities.Model.DTOs;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentRentingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IMapper _mapper;
        private readonly IValidator<BookingDTO> _bookingValidator;

        public BookingsController(IBookingService bookingService, IMapper mapper, IValidator<BookingDTO> bookingValidator)
        {
            _bookingService = bookingService;
            _mapper = mapper;
            _bookingValidator = bookingValidator;
        }

        /// <summary>
        /// Get All Bookings
        /// </summary>
        /// <returns>An IActionResult containing the list of all bookings.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await _bookingService.GetAllAsync();
            var bookingDTOs = _mapper.Map<IEnumerable<BookingDTO>>(bookings);
            return Ok(bookingDTOs);
        }

        /// <summary>
        /// Get a Booking by ID
        /// </summary>
        /// <param name="id">The ID of the booking to retrieve.</param>
        /// <returns>An IActionResult containing the booking information.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            var booking = await _bookingService.GetByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            var bookingDTO = _mapper.Map<BookingDTO>(booking);
            return Ok(bookingDTO);
        }

        /// <summary>
        /// Add a new Booking
        /// </summary>
        /// <param name="dto">The BookingDTO object containing the booking details.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPost]
        public async Task<IActionResult> AddBooking([FromBody] BookingDTO dto)
        {
            var validationResult = await _bookingValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            await _bookingService.AddAsync(dto);
            return Ok("Booking added successfully");
        }

        /// <summary>
        /// Update an existing Booking
        /// </summary>
        /// <param name="id">The ID of the booking to update.</param>
        /// <param name="dto">The BookingDTO object containing the updated booking details.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(int id, [FromBody] BookingDTO dto)
        {
            var validationResult = await _bookingValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            try
            {
                var existingBooking = await _bookingService.GetByIdAsync(id);
                if (existingBooking == null)
                {
                    return NotFound("Booking not found");
                }

                // Update only the properties that can be modified
                existingBooking.BookingDate = dto.BookingDate;
                existingBooking.CheckInDate = dto.CheckInDate;
                existingBooking.CheckOutDate = dto.CheckOutDate;
                existingBooking.PaymentStatus = dto.PaymentStatus;

                await _bookingService.UpdateAsync(id, _mapper.Map<BookingDTO>(existingBooking));
                return Ok("Booking updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete a Booking by ID
        /// </summary>
        /// <param name="id">The ID of the booking to delete.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            try
            {
                await _bookingService.DeleteAsync(id);
                return Ok("Booking deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}