using BookService.Data;
using BookService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BookService.Messaging; // Asegúrate que esta referencia esté bien puesta

namespace BookService.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BookingController : ControllerBase
{
    private readonly BookDbContext _context;
    private readonly RabbitMqPublisher _publisher;

    public BookingController(BookDbContext context, RabbitMqPublisher publisher)
    {
        _context = context;
        _publisher = publisher;
    }

    [HttpPost]
    public async Task<IActionResult> CreateBooking(Booking booking)
    {
        booking.CreatedAt = DateTime.UtcNow;
        booking.UpdatedAt = DateTime.UtcNow;

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        string message = $"Reservierung erstellt: {booking.PassengerFirstname} {booking.PassengerLastname}, Flug-ID: {booking.FlightId}, Tickets: {booking.TicketCount}";
        _publisher.Send(message);

        return CreatedAtAction(nameof(GetBookingById), new { id = booking.Id }, booking);
    }

    [HttpGet]
    public ActionResult<IEnumerable<Booking>> GetAllBookings()
    {
        return Ok(_context.Bookings.ToList());
    }

    [HttpGet("{id}")]
    public ActionResult<Booking> GetBookingById(int id)
    {
        var booking = _context.Bookings.FirstOrDefault(b => b.Id == id);
        if (booking == null)
            return NotFound();
        return Ok(booking);
    }
}
