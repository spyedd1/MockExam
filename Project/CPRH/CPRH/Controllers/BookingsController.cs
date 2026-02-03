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
            var BookingData = await _context.Booking.FirstOrDefaultAsync(r => r.BookingId == id && r.UserId == UserId);
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
        public IActionResult Create()
        {
            ViewData["RoomId"] = new SelectList(_context.Set<Room>(), "RoomId", "RoomId");
            return View();
        }

        [Authorize]
        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingId,BookingDate,StartTime,EndTime,Status")] Booking booking)
        {

            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Claims

            if (UserId == null)
            {
                return NotFound();
            }

            booking.UserId = UserId;

            ModelState.Remove("UserId");

            var RoomId = await _context.Room.FindAsync(booking.RoomId);

            if (RoomId == null)
            {
                return NotFound();
            }

            booking.Room = RoomId;
            ModelState.Remove("RoomId");

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
            var BookingData = await _context.Booking.FirstOrDefaultAsync(r => r.BookingId == id && r.UserId == UserId);
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

        [Authorize]
        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingId,BookingDate,StartTime,EndTime,Status")] Booking booking)
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Claims

            if (UserId == null)
            {
                return NotFound();
            }

            booking.UserId = UserId;

            ModelState.Remove("UserId");

            var room = await _context.Room.FindAsync(booking.RoomId);

            if (room == null)
            {
                return NotFound();
            }

            booking.Room = room;
            ModelState.Remove("RoomId");


            if (id != booking.BookingId)
            {
                return NotFound();
            }

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
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
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
