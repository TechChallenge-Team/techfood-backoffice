namespace TechFood.BackOffice.Application.Common.Data;

public class Coordinates(string latitude, string longitude)
{
    public string Latitude { get; } = latitude;

    public string Longitude { get; } = longitude;
}
