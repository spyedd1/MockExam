namespace CPRH.Models
{
    public class Booking
    {
        public int BookingId { get; set; }

        public int RoomId { get; set; } // FK

        public string UserId { get; set; } // FK

        public DateTime BookingDate { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public string Status { get; set; }

        // Navigation properties

        public Room Room { get; set; }
    }
}
