using FlightService.Data;
using FlightService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightService.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] 
public class FlightController : ControllerBase
{
    private readonly FlightServiceDb _db;

    public FlightController(FlightServiceDb db)
    {
        _db = db;
    }

    [HttpPost]
    public IActionResult CreateFlight([FromBody] Flight flight)
    {
        _db.Create(flight);
        return CreatedAtAction(nameof(GetFlight), new { id = flight.Id }, flight);
    }

    [HttpGet]
    public ActionResult<List<Flight>> GetFlights()
    {
        return Ok(_db.Get());
    }

    [HttpGet("{id}")]
    public ActionResult<Flight> GetFlight(string id)
    {
        var flight = _db.Get(id);
        if (flight == null)
            return NotFound();
        return Ok(flight);
    }
}
