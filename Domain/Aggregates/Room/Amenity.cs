namespace Domain.Aggregates.Room;
public class Amenity
{
    public string Value { get; private set; } = null!;

    private Amenity() { } // EF Core

    public Amenity(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Amenity cannot be empty.");

        Value = value;
    }
}