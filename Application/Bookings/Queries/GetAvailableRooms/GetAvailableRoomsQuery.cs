namespace Application.Bookings.Queries.GetAvailableRooms;

public class GetAvailableRoomsQuery
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Guests { get; set; }
}