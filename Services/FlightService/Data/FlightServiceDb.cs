using FlightService.Models;
using FlightService.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace FlightService.Data;

public class FlightServiceDb
{
    private readonly IMongoCollection<Flight> _flights;

    public FlightServiceDb(IOptions<FlightDatabaseSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _flights = database.GetCollection<Flight>(settings.Value.FlightsCollectionName);
    }

    public List<Flight> Get() => _flights.Find(f => true).ToList();

    public Flight? Get(string id) => _flights.Find(f => f.Id == id).FirstOrDefault();

    public void Create(Flight flight) => _flights.InsertOne(flight);
}
