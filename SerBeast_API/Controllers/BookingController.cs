using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SerBeast_API.Data;
using SerBeast_API.Model;
using SerBeast_API.Model.Dto;
using System.Net;
using System.Security.Claims;

namespace SerBeast_API.Controllers
{
    [Route("api/Booking")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private ApiResponse _response;

        public BookingController(ApplicationDbContext db)
        {
            _db = db;
            _response = new ApiResponse();
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] BookingCreateDTO bookingCreateDTO)
        {
            if (bookingCreateDTO == null)
            {
                return BadRequest("Booking details are required.");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not found.");
            }

            var newBooking = new Booking
            {
                UserId = userId,
                ProfessionalServiceId = bookingCreateDTO.ProfessionalServiceId,
                BookingDate = bookingCreateDTO.BookingDate,
            };

            try
            {
                _db.Bookings.Add(newBooking);
                await _db.SaveChangesAsync();

                // Return a 201 Created response with a location header
                _response.Result = newBooking;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtAction(nameof(GetBookingById), new { id = newBooking.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("An error occurred while creating the booking.");
                _response.ErrorMessages.Add(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(string id)
        {
            var booking = await _db.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound($"Booking with ID {id} not found.");
            }

            _response.Result = booking;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        // GetBookingsForProfessional - Fetches all bookings for a professional
        [HttpGet("Professional/{professionalId}")]
        public async Task<IActionResult> GetBookingsForProfessional(string professionalId)
        {
            var bookings = await _db.Bookings
                .Where(b => b.ProfessionalService.ProfessionalId == professionalId)
                .ToListAsync();

            if (bookings.Count == 0)
            {
                return NoContent(); 
            }

            _response.Result = bookings;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        // GetBookingsForUser - Fetches all bookings for a User
        [HttpGet("User/{UserId}")]
        public async Task<IActionResult> GetBookingsForUser(string userId)
        {
            var bookings = await _db.Bookings
                .Where(b => b.UserId == userId)
                .ToListAsync();

            if (bookings.Count == 0)
            {
                return NoContent(); 
            }

            _response.Result = bookings;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }







        // UpdateBookingStatus - Allows updating the status of a booking
        //[HttpPut("{id}/status")]
        //public async Task<IActionResult> UpdateBookingStatus(string id, [FromBody] string status)
        //{
        //    var booking = await _db.Bookings.FindAsync(id);
        //    if (booking == null)
        //    {
        //        return NotFound($"Booking with ID {id} not found.");
        //    }

        //    booking.Status = status;
        //    await _db.SaveChangesAsync();

        //    _response.Result = booking;
        //    _response.StatusCode = HttpStatusCode.OK;
        //    return Ok(_response);
        //}
    }
}
