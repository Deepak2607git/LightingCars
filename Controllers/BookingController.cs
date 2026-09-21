using LightningCars.Api.DTOs;
using LightningCars.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalCar.Api.Data;
using System.Security.Claims;

namespace LightningCars.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BookingsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BookingsController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/Bookings
        [HttpPost]
        public async Task<IActionResult> CreateBooking(
            [FromBody] CreateBookingRequest request)
        {
            // Validate car type
            if (string.IsNullOrWhiteSpace(request.CarType))
            {
                return BadRequest(new
                {
                    message = "Car type is required."
                });
            }

            // Validate pickup date/time
            if (request.PickupDate <= DateTime.UtcNow)
            {
                return BadRequest(new
                {
                    message = "Pickup date and time must be in the future."
                });
            }

            // Validate drop date/time
            if (request.DropDate <= request.PickupDate)
            {
                return BadRequest(new
                {
                    message = "Drop date and time must be after pickup date and time."
                });
            }

            // Validate from location
            if (string.IsNullOrWhiteSpace(request.FromLocation))
            {
                return BadRequest(new
                {
                    message = "From location is required."
                });
            }

            // Validate to location
            if (string.IsNullOrWhiteSpace(request.ToLocation))
            {
                return BadRequest(new
                {
                    message = "To location is required."
                });
            }

            // Validate phone number
            if (string.IsNullOrWhiteSpace(request.PhoneNumber))
            {
                return BadRequest(new
                {
                    message = "Phone number is required."
                });
            }

            // Get logged-in user's email from JWT
            var email = User.FindFirstValue(ClaimTypes.Email);

            // Fallback if the JWT uses "email" instead
            if (string.IsNullOrWhiteSpace(email))
            {
                email = User.FindFirstValue("email");
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                return Unauthorized(new
                {
                    message = "Unable to identify logged-in user."
                });
            }

            // Create booking
            var booking = new Booking
            {
                CarType = request.CarType,

                PickupDate = request.PickupDate,
                DropDate = request.DropDate,

                FromLocation = request.FromLocation,
                ToLocation = request.ToLocation,

                PhoneNumber = request.PhoneNumber,

                // Taken from logged-in user's JWT
                Email = email,

                // New bookings always start as PENDING
                Status = "PENDING",

                // Always store creation time in UTC
                CreatedAt = DateTime.UtcNow
            };

            _context.Bookings.Add(booking);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Booking created successfully.",
                booking
            });
        }


        // GET: api/Bookings/my
        [HttpGet("my")]
        public async Task<IActionResult> GetMyBookings()
        {
            // Get logged-in user's email from JWT
            var email = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrWhiteSpace(email))
            {
                email = User.FindFirstValue("email");
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                return Unauthorized(new
                {
                    message = "Unable to identify logged-in user."
                });
            }

            // Return only bookings belonging to logged-in user
            var bookings = await _context.Bookings
                .Where(x => x.Email == email)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            return Ok(bookings);
        }


        // GET: api/Bookings/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBooking(int id)
        {
            // Get logged-in user's email
            var email = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrWhiteSpace(email))
            {
                email = User.FindFirstValue("email");
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                return Unauthorized(new
                {
                    message = "Unable to identify logged-in user."
                });
            }

            // User can only access their own booking
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.Email == email);

            if (booking == null)
            {
                return NotFound(new
                {
                    message = "Booking not found."
                });
            }

            return Ok(booking);
        }
    }
}