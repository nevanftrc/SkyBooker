namespace FlightService.Settings;

public class FlightDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string FlightsCollectionName { get; set; } = null!;
}
