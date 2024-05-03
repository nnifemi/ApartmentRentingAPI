using ApartmentBLL;
using ApartmentDAL;
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
    public class ApartmentsController : ControllerBase
    {
        private readonly IApartmentService _apartmentService;
        private readonly IBookingService _bookingService;
        private readonly IMapper _mapper;
        private readonly IValidator<ApartmentDTO> _apartmentValidator;

        public ApartmentsController(IApartmentService apartmentService, IBookingService bookingService, IMapper mapper, IValidator<ApartmentDTO> apartmentValidator)
        {
            _apartmentService = apartmentService;
            _bookingService = bookingService;
            _mapper = mapper;
            _apartmentValidator = apartmentValidator;
        }

        /// <summary>
        /// Get All Apartments
        /// </summary>
        /// <returns>An IActionResult containing the list of all apartments.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllApartments()
        {
            var apartments = await _apartmentService.GetAllAsync();
            var apartmentDTOs = _mapper.Map<IEnumerable<ApartmentDTO>>(apartments);
            return Ok(apartmentDTOs);
        }

        /// <summary>
        /// Get an Apartment by ID
        /// </summary>
        /// <param name="id">The ID of the apartment to retrieve.</param>
        /// <returns>An IActionResult containing the apartment information.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetApartmentById(int id)
        {
            var apartment = await _apartmentService.GetByIdAsync(id);
            if (apartment == null)
            {
                return NotFound();
            }
            var apartmentDTO = _mapper.Map<ApartmentDTO>(apartment);
            return Ok(apartmentDTO);
        }

        /// <summary>
        /// Add a new Apartment
        /// </summary>
        /// <param name="dto">The ApartmentDTO object containing the apartment details.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPost]
        public async Task<IActionResult> AddApartment([FromBody] ApartmentDTO dto)
        {
            // Validate the incoming data using the ApartmentValidator
            var validationResult = await _apartmentValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            await _apartmentService.AddAsync(dto);
            return Ok("Apartment added successfully");
        }

        /// <summary>
        /// Update an existing Apartment
        /// </summary>
        /// <param name="id">The ID of the apartment to update.</param>
        /// <param name="dto">The ApartmentDTO object containing the updated apartment details.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateApartment(int id, [FromBody] ApartmentDTO dto)
        {
            // Validate the incoming data using the ApartmentValidator
            var validationResult = await _apartmentValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            try
            {
                // Retrieve the existing apartment
                var existingApartment = await _apartmentService.GetByIdAsync(id);
                if (existingApartment == null)
                {
                    return NotFound("Apartment not found");
                }

                // Update the properties that are allowed to be updated
                existingApartment.Location = dto.Location;
                existingApartment.FlatNumber = dto.FlatNumber;
                existingApartment.Description = dto.Description;
                existingApartment.Amenities = dto.Amenities;
                existingApartment.Price = dto.Price;
                existingApartment.Availability = dto.Availability;

                // Update the apartment
                await _apartmentService.UpdateAsync(id, _mapper.Map<ApartmentDTO>(existingApartment));

                return Ok("Apartment updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete an Apartment by ID
        /// </summary>
        /// <param name="id">The ID of the apartment to delete.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApartment(int id)
        {
            try
            {
                await _apartmentService.DeleteAsync(id);
                return Ok("Apartment deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Search Apartments based on criteria
        /// </summary>
        /// <param name="location">The location to search for.</param>
        /// <param name="minPrice">The minimum price.</param>
        /// <param name="maxPrice">The maximum price.</param>
        /// <param name="amenities">The array of amenities to search for.</param>
        /// <returns>An IActionResult containing the list of matching apartments.</returns>
        [HttpGet("search")]
        public async Task<IActionResult> SearchApartments([FromQuery] string location, decimal minPrice, decimal maxPrice, [FromQuery] string[] amenities)
        {
            var apartments = await _apartmentService.SearchApartments(location, minPrice, maxPrice, amenities);
            var apartmentDTOs = _mapper.Map<IEnumerable<ApartmentDTO>>(apartments);
            return Ok(apartmentDTOs);
        }

        /// <summary>
        /// Book an Apartment
        /// </summary>
        /// <param name="id">The ID of the apartment to book.</param>
        /// <param name="bookingDTO">The BookingDTO object containing the booking details.</param>
        /// <returns>An IActionResult indicating the result of the booking operation.</returns>
        [HttpPost("{id}/book")]
        public async Task<IActionResult> BookApartment(int id, [FromBody] BookingDTO bookingDTO)
        {
            try
            {
                // Validate booking data
                if (bookingDTO == null || bookingDTO.CheckInDate == default || bookingDTO.CheckOutDate == default || bookingDTO.UserID == 0)
                {
                    return BadRequest("Invalid booking data");
                }

                // Retrieve the apartment by ID
                var apartment = await _apartmentService.GetByIdAsync(id);
                if (apartment == null)
                {
                    return NotFound("Apartment not found");
                }

                // Check if the apartment is available for the requested dates
                var isAvailable = await IsApartmentAvailableForBooking(id, bookingDTO.CheckInDate, bookingDTO.CheckOutDate);
                if (!isAvailable)
                {
                    return BadRequest("The apartment is not available for the requested dates");
                }

                // Create a new booking
                var booking = new BookingDTO
                {
                    UserID = bookingDTO.UserID,
                    ApartmentID = id,
                    BookingDate = DateTime.Now,
                    CheckInDate = bookingDTO.CheckInDate,
                    CheckOutDate = bookingDTO.CheckOutDate,
                    PaymentStatus = "Pending" // You can update this based on your payment flow
                };

                // Add the booking
                await _bookingService.AddAsync(booking);

                // Update the apartment availability status
                apartment.Availability = false; // Assuming the apartment is no longer available after booking
                await _apartmentService.UpdateAsync(id, _mapper.Map<ApartmentDTO>(apartment));

                return Ok("Apartment booked successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private async Task<bool> IsApartmentAvailableForBooking(int apartmentId, DateTime checkInDate, DateTime checkOutDate)
        {
            // Retrieve bookings for the apartment within the requested date range
            var bookings = await _apartmentService.GetBookingsByApartmentId(apartmentId);

            // Check if there are any overlapping bookings
            foreach (var booking in bookings)
            {
                if (checkInDate >= booking.CheckInDate && checkInDate <= booking.CheckOutDate
                    || checkOutDate >= booking.CheckInDate && checkOutDate <= booking.CheckOutDate)
                {
                    return false; // Apartment is not available for booking
                }
            }

            return true; // Apartment is available for booking
        }
    }
}