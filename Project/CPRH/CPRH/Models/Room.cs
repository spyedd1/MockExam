namespace CPRH.Models
{
    public class Room
    {
        public int RoomId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Capacity { get; set; }

        public string Equipment { get; set; }

        public float PricePerHour { get; set; }

        public int StarRating { get; set; }

        public double GuestRating { get; set; }

        public int NumberOfRooms { get; set; }

        public bool IsAvailable { get; set; }

        // Navigation property

        public ICollection<Booking>? Bookings { get; set; }
    }
}
