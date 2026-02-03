using CPRH.Models;
using Microsoft.EntityFrameworkCore;

namespace CPRH.Data
{
    public class SeedData
    {
        public static async Task SeedStaffAsync(ApplicationDbContext context)
        {
            if (!await context.Staff.AnyAsync())
            {
                var staff = new List<Staff>
                {
                    new Staff
                    {
                        FirstName = "John",
                        StaffEmail = "johncitypointstaff@gmail.com",
                        StaffPhoneNumber = "07673648132",
                        Role = "Manager"
                    },
                    new Staff
                    {
                        FirstName = "Samantha",
                        StaffEmail = "samanthacitypointstaff@gmail.com",
                        StaffPhoneNumber = "07899537543",
                        Role = "Co-Executive"
                    },
                    new Staff
                    {
                        FirstName = "Paul",
                        StaffEmail = "paulcitypointstaff@gmail.com",
                        StaffPhoneNumber = "07573935762",
                        Role = "Customer Support"
                    },
                    new Staff
                    {
                        FirstName = "Bob",
                        StaffEmail = "bobcitypointstaff@gmail.com",
                        StaffPhoneNumber = "07932568132",
                        Role = "Customer Support"
                    },
                    new Staff
                    {
                        FirstName = "Max",
                        StaffEmail = "maxcitypointstaff@gmail.com",
                        StaffPhoneNumber = "07676548932",
                        Role = "Receptionist"
                    },
                };
                await context.Staff.AddRangeAsync(staff);
                await context.SaveChangesAsync();
            }
        }

        public static async Task SeedRoomAsync(ApplicationDbContext context)
        {
            if (!await context.Room.AnyAsync())
            {
                var rooms = new List<Room>
                {
                    new Room
                    {
                        Name = "London Bridge View",
                        Description = "Modern boardroom with stunning Thames views and plenty of natural light",
                        Capacity = 12,
                        Equipment = "4K projector, interactive whiteboard, video conferencing system, air con",
                        PricePerHour = 48,
                        StarRating = 5,
                        GuestRating = 4.8,
                        NumberOfRooms = 2,
                        IsAvailable = true
                    },
                    new Room
                    {
                        Name = "Manchester Central",
                        Description = "Spacious workshop room perfect for team brainstorming sessions",
                        Capacity = 18,
                        Equipment = "Large touch screen, flipcharts, coffee station, superfast WiFi",
                        PricePerHour = 55,
                        StarRating = 4,
                        GuestRating = 4.6,
                        NumberOfRooms = 3,
                        IsAvailable = true
                    },
                    new Room
                    {
                        Name = "Edinburgh Waverley",
                        Description = "Quiet meeting pod ideal for interviews or focused calls",
                        Capacity = 6,
                        Equipment = "HD webcam, noise-cancelling speakerphone, ergonomic chairs",
                        PricePerHour = 30,
                        StarRating = 4,
                        GuestRating = 4.7,
                        NumberOfRooms = 1,
                        IsAvailable = true
                    },
                    new Room
                    {
                        Name = "Birmignham Library",
                        Description = "Versatile training room with flexible seating arrangements",
                        Capacity = 15,
                        Equipment = "Dual projectors, HDMI connectivity, daylight blinds",
                        PricePerHour = 38,
                        StarRating = 4,
                        GuestRating = 4.4,
                        NumberOfRooms = 2,
                        IsAvailable = true
                    },
                    new Room
                    {
                        Name = "Glasgow Riverside",
                        Description = "Creative breakout space with relaxed furniture and river views",
                        Capacity = 10,
                        Equipment = "65-inch monitor, soft seating, whiteboard wall, power everywhere",
                        PricePerHour = 38,
                        StarRating = 4,
                        GuestRating = 4.4,
                        NumberOfRooms = 2,
                        IsAvailable = true
                    },
                    new Room
                    {
                        Name = "Oxford High Street",
                        Description = "Premium executive suite with luxury finishes and city views",
                        Capacity = 10,
                        Equipment = "4K laser projector, surround sound, smart lighting, minibar",
                        PricePerHour = 78,
                        StarRating = 5,
                        GuestRating = 4.9,
                        NumberOfRooms = 1,
                        IsAvailable = false
                    },
                    new Room
                    {
                        Name = "Cambridge Kings",
                        Description = "Formal lecture-style room with tiered seating and AV setup",
                        Capacity = 24,
                        Equipment = "Lectern with mic, ceiling microphones, full recording system",
                        PricePerHour = 85,
                        StarRating = 5,
                        GuestRating = 4.7,
                        NumberOfRooms = 3,
                        IsAvailable = false
                    },
                    new Room
                    {
                        Name = "Bristol Harbourside",
                        Description = "Relaxed waterfront room with harbour-inspired decor",
                        Capacity = 8,
                        Equipment = "TV screen, bean bags, Nespresso machine, Bluetooth speaker",
                        PricePerHour = 34,
                        StarRating = 4,
                        GuestRating = 4.5,
                        NumberOfRooms = 2,
                        IsAvailable = false
                    },
                    new Room
                    {
                        Name = "Cardiff Millennium",
                        Description = "Contemporary multi-purpose room near the bay",
                        Capacity = 16,
                        Equipment = "Video wall option, dual screens, catering pass-through",
                        PricePerHour = 52,
                        StarRating = 5,
                        GuestRating = 4.6,
                        NumberOfRooms = 5,
                        IsAvailable = false
                    },
                    new Room
                    {
                        Name = "Leeds Victoria",
                        Description = "Characterful room in historic building with high ceilings",
                        Capacity = 14,
                        Equipment = "Projector & screen, vintage chandeliers, enhanced WiFi",
                        PricePerHour = 46,
                        StarRating = 4,
                        GuestRating = 4.6,
                        NumberOfRooms = 1,
                        IsAvailable = false
                    },
                };
                await context.Room.AddRangeAsync(rooms);
                await context.SaveChangesAsync();
            }
        }
    }
}
