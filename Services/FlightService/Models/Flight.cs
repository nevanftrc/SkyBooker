using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FlightService.Models;

public class Flight
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("flightId")]
    public string FlightId { get; set; } = string.Empty;

    [BsonElement("airlineName")]
    public string AirlineName { get; set; } = string.Empty;

    [BsonElement("source")]
    public string Source { get; set; } = string.Empty;

    [BsonElement("destination")]
    public string Destination { get; set; } = string.Empty;

    [BsonElement("departure_time")]
    public DateTime DepartureTime { get; set; }

    [BsonElement("arrival_time")]
    public DateTime ArrivalTime { get; set; }

    [BsonElement("available_seats")]
    public int AvailableSeats { get; set; }

    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
