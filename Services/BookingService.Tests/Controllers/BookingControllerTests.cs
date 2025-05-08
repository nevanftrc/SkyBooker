using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using BookService.Controllers;
using BookService.Data;
using BookService.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace BookingService.Tests.Controllers
{
    public class BookingControllerTests
    {
        private BookingController _controller;
        private BookDbContext _context;

        public BookingControllerTests()
        {
            var options = new DbContextOptionsBuilder<BookDbContext>()
                .UseInMemoryDatabase(databaseName: "BookingDb_Test")
                .Options;

            _context = new BookDbContext(options);
            _context.Database.EnsureCreated();

            _controller = new BookingController(_context);
        }

        [Fact]
        public async Task CreateBooking_ReturnsCreatedAtAction()
        {
            var booking = new Booking
            {
                PassengerFirstname = "Test",
                PassengerLastname = "User",
                FlightId = 1,
                TicketCount = 2
            };

            var result = await _controller.CreateBooking(booking);

            var createdAtResult = Assert.IsType<CreatedAtActionResult>(result);
            var createdBooking = Assert.IsType<Booking>(createdAtResult.Value);

            Assert.Equal("Test", createdBooking.PassengerFirstname);
            Assert.Equal(2, _context.Bookings.Count());
        }

        [Fact]
        public void GetAllBookings_ReturnsOkWithList()
        {
            var result = _controller.GetAllBookings();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var bookings = Assert.IsType<List<Booking>>(okResult.Value);

            Assert.NotNull(bookings);
        }

        [Fact]
        public void GetBookingById_ReturnsBookingOrNotFound()
        {
            var booking = new Booking
            {
                PassengerFirstname = "Anna",
                PassengerLastname = "Smith",
                FlightId = 2,
                TicketCount = 1
            };

            _context.Bookings.Add(booking);
            _context.SaveChanges();

            var result = _controller.GetBookingById(booking.Id);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var resultBooking = Assert.IsType<Booking>(okResult.Value);

            Assert.Equal("Anna", resultBooking.PassengerFirstname);
        }
    }
}
