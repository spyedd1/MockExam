using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CPRH.Data;
using CPRH.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace CPRH.Controllers
{
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
        // GET: Bookings
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userBookingData = await _context.Booking.Where(h => h.UserId == userId).ToListAsync();

            return View(userBookingData);
        }

        [Authorize]
        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var BookingData = await _context.Booking.FirstOrDefaultAsync(r => r.BookingId == id && (r.UserId == UserId || User.IsInRole("Admin")));
            if (BookingData == null)
            {
                return Unauthorized();
            }
            return View(BookingData);

            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .Include(b => b.Room)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        [Authorize]
        // GET: Bookings/Create
        public async Task<IActionResult> Create(int roomId)
        {
            var room = await _context.Room.FindAsync(roomId);

            if (room == null)
            {
                return NotFound();
            }

            if (!room.IsAvailable)
            {
                TempData["ErrorMessage"] = "This room is unavailable.";
                return RedirectToAction("Details", "Rooms", new { id = roomId });
            }

            var booking = new Booking
            {
                RoomId = roomId,
                Status = "Pending"   // ← add this line
                                     // You can also set BookingDate = DateTime.Today if useful
            };

            ViewBag.RoomId = roomId;
            return View(booking);
        }

        [Authorize]
        // POST: Bookings/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingId,RoomId,BookingDate,StartTime,EndTime,Status")] Booking booking)
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (UserId == null)
            {
                return NotFound();
            }

            var room = await _context.Room.FindAsync(booking.RoomId);
            if (room == null)
            {
                return NotFound();
            }

            if (!room.IsAvailable)
            {
                ModelState.AddModelError(string.Empty, "This room is unavailable.");
                ViewData["RoomId"] = new SelectList(_context.Set<Room>(), "RoomId", "RoomId", booking.RoomId);
                return View(booking);
            }

            booking.UserId = UserId;
            ModelState.Remove("UserId");

            // ── STEP 3: Force Pending for non-admins (defense in depth) ──
            if (!User.IsInRole("Admin"))
            {
                booking.Status = "Pending";
            }

            var RoomId = await _context.Room.FindAsync(booking.RoomId);
            if (RoomId == null)
            {
                return NotFound();
            }

            booking.Room = RoomId;
            ModelState.Remove("Room");

            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["RoomId"] = new SelectList(_context.Set<Room>(), "RoomId", "RoomId", booking.RoomId);
            return View(booking);
        }

        [Authorize]
        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var BookingData = await _context.Booking.FirstOrDefaultAsync(r => r.BookingId == id && (r.UserId == UserId || User.IsInRole("Admin")));
            if (BookingData == null)
            {
                return Unauthorized();
            }
            return View(BookingData);




            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewData["RoomId"] = new SelectList(_context.Set<Room>(), "RoomId", "RoomId", booking.RoomId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingId,RoomId,BookingDate,StartTime,EndTime,Status")] Booking booking)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return NotFound();

            var existingBooking = await _context.Booking.AsNoTracking().FirstOrDefaultAsync(r => r.BookingId == id &&(r.UserId == userId || User.IsInRole("Admin")));

            if (existingBooking == null)
                return NotFound();

            booking.UserId = existingBooking.UserId;

            ModelState.Remove("UserId");
            ModelState.Remove("Room");

            var room = await _context.Room.FindAsync(booking.RoomId);
            if (room == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingId))
                        return NotFound();
                    else
                        throw;
                }

                if (User.IsInRole("Admin"))
                    return RedirectToAction("Index", "Admin");

                return RedirectToAction(nameof(Index));
            }


            ViewData["RoomId"] = new SelectList(_context.Set<Room>(), "RoomId", "RoomId", booking.RoomId);
            return View(booking);
        }



        [Authorize]
        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .Include(b => b.Room)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        [Authorize]
        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Booking.FindAsync(id);
            if (booking != null)
            {
                _context.Booking.Remove(booking);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Booking.Any(e => e.BookingId == id);
        }
    }
}
