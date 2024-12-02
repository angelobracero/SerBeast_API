using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SerBeast_API.Data;
using SerBeast_API.Model;
using SerBeast_API.Model.Dto;
using SerBeast_API.Utilities;
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


            var newBooking = new Booking
            {
                UserId = bookingCreateDTO.UserId,
                ProfessionalServiceId = bookingCreateDTO.ProfessionalServiceId,
                BookingDate = bookingCreateDTO.BookingDate.Add(bookingCreateDTO.BookingTime)
            };

            try
            {
                _db.Bookings.Add(newBooking);
                await _db.SaveChangesAsync();

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


        [HttpGet("User/{userId}")]
        public async Task<IActionResult> GetBookingsForUser(string userId)
        {
            try
            {
                var bookings = await _db.Bookings
                    .Where(b => b.UserId == userId)
                    .AsNoTracking()
                    .Include(b => b.ProfessionalService)
                    .ThenInclude(ps => ps.Professional)
                    .Include(b => b.ProfessionalService)
                    .ThenInclude(ps => ps.Service)
                    .ToListAsync();

                if (bookings.Count == 0)
                {
                    _response.ErrorMessages.Add("No bookings found for this user.");
                    return NoContent();
                }

                var result = bookings.Select(b => new
                {
                    BookingId = b.Id,
                    Status = b.Status.ToString(),
                    Service = b.ProfessionalService?.Service?.Title ?? "No Service",
                    Professional = new
                    {
                        ProfessionalName = (b.ProfessionalService?.Professional?.FirstName ?? "Unknown") + " " +
                                           (b.ProfessionalService?.Professional?.LastName ?? "Name")
                    },
                    Date = b.BookingDate.ToString("yyyy-MM-dd"),
                    Time = b.BookingDate.ToString("hh:mm tt")
                }).ToList();

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = result;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                // Capture any error that occurs and add it to ErrorMessages
                _response.ErrorMessages.Add($"Error occurred: {ex.Message}");
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                return StatusCode(500, _response);
            }
        }

        [HttpGet("Professional/{proId}")]
        public async Task<IActionResult> GetBookingsForProfessional(string proId)
        {
            try
            {
                var bookings = await _db.Bookings
                    .Where(b => b.ProfessionalService.ProfessionalId == proId)
                    .AsNoTracking()
                    .Include(b => b.ProfessionalService)
                    .ThenInclude(ps => ps.Professional)
                    .Include(b => b.ProfessionalService)
                    .ThenInclude(ps => ps.Service)
                    .ToListAsync();

                if (bookings.Count == 0)
                {
                    _response.ErrorMessages.Add("No bookings found for this user.");
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.IsSuccess = true;
                    _response.Result = new List<object>(); 
                    return Ok(_response);
                }

                var result = bookings.Select(b => new
                {
                    BookingId = b.Id,
                    Status = b.Status.ToString(),
                    Service = b.ProfessionalService?.Service?.Title ?? "No Service",
                    Professional = new
                    {
                        ProfessionalName = (b.ProfessionalService?.Professional?.FirstName ?? "Unknown") + " " +
                                           (b.ProfessionalService?.Professional?.LastName ?? "Name")
                    },  
                    Date = b.BookingDate.ToString("yyyy-MM-dd"),
                    Time = b.BookingDate.ToString("hh:mm tt")
                }).ToList();

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = result;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                // Capture any error that occurs and add it to ErrorMessages    
                _response.ErrorMessages.Add($"Error occurred: {ex.Message}");
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                return StatusCode(500, _response);
            }
        }



        [HttpGet("Professional/{proId}/dashboard")]
        public async Task<IActionResult> GetDataForDashboard(string proId)
        {
            try
            {
                // Fetch all bookings for the professional with necessary navigation properties
                var bookings = await _db.Bookings
                    .Where(b => b.ProfessionalService.ProfessionalId == proId)
                    .AsNoTracking()
                    .Include(b => b.ProfessionalService)
                        .ThenInclude(ps => ps.Professional)
                    .Include(b => b.ProfessionalService.Service)
                    .OrderBy(b => b.BookingDate) // Sort by BookingDate descending
                    .ToListAsync();

                // Count services offered by the professional
                var serviceOffered = await _db.ProfessionalServices
                    .Where(ps => ps.ProfessionalId == proId)
                    .AsNoTracking()
                    .CountAsync();

                // Compute metrics
                var confirmedBookings = bookings.Count(b => b.Status == BookingStatus.Confirmed);
                var completedServices = bookings.Count(b => b.Status == BookingStatus.Completed);
                var totalEarnings = bookings
                    .Where(b => b.Status == BookingStatus.Completed)
                    .Sum(b => b.ProfessionalService.Price);

                // Project upcoming bookings
                var upcomingBookings = bookings.Select(b => new
                {
                    BookingId = b.Id,
                    Status = b.Status.ToString(),
                    Service = b.ProfessionalService?.Service?.Title ?? "No Service",
                    Professional = $"{b.ProfessionalService?.Professional?.FirstName ?? "Unknown"} " +
                                   $"{b.ProfessionalService?.Professional?.LastName ?? "Name"}",
                    Date = b.BookingDate.ToString("MMM dd yyyy"),
                    Time = b.BookingDate.ToString("hh:mm tt")
                }).ToList();

                // Construct the response
                _response.Result = new
                {
                    ServiceOffered = serviceOffered,
                    ConfirmedBookings = confirmedBookings,
                    CompletedServices = completedServices,
                    TotalEarnings = totalEarnings,
                    UpcomingBookings = upcomingBookings
                };
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                // Handle exceptions
                _response.ErrorMessages.Add($"Error occurred: {ex.Message}");
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                return StatusCode(500, _response);
            }
        }





        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateBookingStatus(string id, [FromBody] string status)
        {
            if (string.IsNullOrWhiteSpace(status))
            {
                return BadRequest("Status is required.");
            }

            var booking = await _db.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound($"Booking with ID {id} not found.");
            }

            // Validate and parse status
            if (!Enum.TryParse<BookingStatus>(status, true, out var bookingStatus))
            {
                return BadRequest($"Invalid status value. Valid statuses are: {string.Join(", ", Enum.GetNames(typeof(BookingStatus)))}.");
            }

            // Update booking status
            booking.Status = bookingStatus;
            await _db.SaveChangesAsync();

            // Prepare response with friendly string
            _response.Result = new
            {
                BookingId = booking.Id,
                UpdatedStatus = booking.Status.ToFriendlyString(),
                Message = "Booking status updated successfully."
            };
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpPut("{id}/status/confirm")]
        public async Task<IActionResult> UpdateBookingStatusToConfirmed(string id)
        {
            var booking = await _db.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound($"Booking with ID {id} not found.");
            }

            if (booking.Status == BookingStatus.Confirmed)
            {
                return BadRequest("The booking is already in 'Confirmed' status.");
            }

            booking.Status = BookingStatus.Confirmed;

            await _db.SaveChangesAsync();

            _response.Result = new
            {
                BookingId = booking.Id,
                UpdatedStatus = booking.Status.ToString(),
                Message = "Booking status updated to 'Confirmed'."
            };

            return Ok(_response);
        }

        [HttpPut("{id}/status/complete")]
        public async Task<IActionResult> UpdateBookingStatusToComplete(string id)
        {
            var booking = await _db.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound($"Booking with ID {id} not found.");
            }

            if (booking.Status == BookingStatus.Completed)
            {
                return BadRequest("The booking is already in 'Confirmed' status.");
            }

            booking.Status = BookingStatus.Completed;

            await _db.SaveChangesAsync();

            _response.Result = new
            {
                BookingId = booking.Id,
                UpdatedStatus = booking.Status.ToString(),
                Message = "Booking status updated to 'Completed'."
            };

            return Ok(_response);
        }

        [HttpPut("{id}/status/cancelled")]
        public async Task<IActionResult> UpdateBookingStatusToCancelled(string id)
        {
            var booking = await _db.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound($"Booking with ID {id} not found.");
            }

            if (booking.Status == BookingStatus.Cancelled)
            {
                return BadRequest("The booking is already in 'Cancelled' status.");
            }

            booking.Status = BookingStatus.Cancelled;

            await _db.SaveChangesAsync();

            _response.Result = new
            {
                BookingId = booking.Id,
                UpdatedStatus = booking.Status.ToString(),
                Message = "Booking status updated to 'Cancelled'."
            };

            return Ok(_response);
        }

        //[HttpPut("{id}/status/completed")]
        //public async Task<IActionResult> UpdateBookingStatusToCompleted(string id)
        //{
        //    var booking = await _db.Bookings.FindAsync(id);
        //    if (booking == null)
        //    {
        //        return NotFound($"Booking with ID {id} not found.");
        //    }

        //    if (booking.Status == BookingStatus.Completed)
        //    {
        //        return BadRequest("The booking is already in 'Cancelled' status.");
        //    }

        //    booking.Status = BookingStatus.Cancelled;

        //    await _db.SaveChangesAsync();

        //    _response.Result = new
        //    {
        //        BookingId = booking.Id,
        //        UpdatedStatus = booking.Status.ToString(),
        //        Message = "Booking status updated to 'Cancelled'."
        //    };

        //    return Ok(_response);
        //}


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(string id)
        {
            var booking = await _db.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound("Booking not found.");
            }

            _db.Bookings.Remove(booking);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
