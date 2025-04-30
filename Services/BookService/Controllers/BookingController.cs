using BookService.Data;
using BookService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookService.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Protegido por JWT
public class BookingController : ControllerBase
{
    private readonly BookDbContext _context;

    public BookingController(BookDbContext context)
    {
        _context = context;
    }

    // POST /api/booking
    [HttpPost]
    public async Task<IActionResult> CreateBooking(Booking booking)
    {
        booking.CreatedAt = DateTime.UtcNow;
        booking.UpdatedAt = DateTime.UtcNow;

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetBookingById), new { id = booking.Id }, booking);
    }

    // GET /api/booking
    [HttpGet]
    public ActionResult<IEnumerable<Booking>> GetAllBookings()
    {
        return Ok(_context.Bookings.ToList());
    }

    // GET /api/booking/{id}
    [HttpGet("{id}")]
    public ActionResult<Booking> GetBookingById(int id)
    {
        var booking = _context.Bookings.FirstOrDefault(b => b.Id == id);
        if (booking == null)
            return NotFound();
        return Ok(booking);
    }
}
