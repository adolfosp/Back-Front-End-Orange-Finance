namespace OrangeFinance.Domain.Farms.ValueObjects;

public struct Coordinate
{
    public Coordinate(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }

    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
}
