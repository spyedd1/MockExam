using CPRH.Data;
using CPRH.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public AdminController(
        ApplicationDbContext context,
        UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }


    public IActionResult Index()
    {
        var allBookings = _context.Booking
            .OrderByDescending(b => b.BookingDate)
            .ToList();

        // this will count the amount of rooms which are available from the viewbag
        ViewBag.AvailableRooms = _context.Room
            .Count(r => r.IsAvailable);

        ViewBag.Users = _userManager.Users.ToList();

        return View(allBookings);
    }

    public async Task<IActionResult> ManageBooking(string search, string status)
    {
        // first get all users
        var users = await _userManager.Users.ToListAsync();
        var userDictionary = users.ToDictionary(u => u.Id, u => u.Email);

        // query bookings
        var bookings = _context.Booking
            .Include(b => b.Room) // this will include the room if its needed
            .AsQueryable();

        // search function
        if (!string.IsNullOrEmpty(search))
        {
            bookings = bookings.Where(b =>
                b.BookingId.ToString().Contains(search) ||
                b.RoomId.ToString().Contains(search) ||
                b.UserId.Contains(search));
        }

        // status filter
        if (!string.IsNullOrEmpty(status) && status != "All")
        {
            bookings = bookings.Where(b => b.Status == status);
        }

        var result = await bookings
            .OrderByDescending(b => b.BookingDate)
            .ToListAsync();

        // create view model or use viewbag for the user emails
        ViewBag.UserEmails = userDictionary;

        // just incase make a list of the view models
        var viewModels = result.Select(b => new BookingWithUserViewModel
        {
            Booking = b,
            UserEmail = userDictionary.ContainsKey(b.UserId) ? userDictionary[b.UserId] : "Email not found"
        }).ToList();

        return View(viewModels); // pass the viewModels
    }

    public async Task<IActionResult> ManageRoom(string search, string status)
    {
        var rooms = _context.Room.AsQueryable();

        // search by name OR description of room
        if (!string.IsNullOrEmpty(search))
        {
            rooms = rooms.Where(r =>
                r.Name.Contains(search) ||
                r.Description.Contains(search));
        }

        // status filter
        if (!string.IsNullOrEmpty(status) && status != "All")
        {
            bool available = status == "Available";
            rooms = rooms.Where(r => r.IsAvailable == available);
        }

        var result = await rooms
            .OrderBy(r => r.Name)
            .ToListAsync();

        return View(result);
    }

    //  class for booking with user email
    public class BookingWithUserViewModel
    {
        public Booking Booking { get; set; }
        public string UserEmail { get; set; }
        public string UserName { get; set; } // incase i add a username
    }
}