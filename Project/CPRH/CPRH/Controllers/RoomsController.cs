using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CPRH.Data;
using CPRH.Models;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace CPRH.Controllers
{
    public class RoomsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoomsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Rooms
        public async Task<IActionResult> Index()
        {
            return View(await _context.Room.ToListAsync());
        }

        // GET: Rooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Room
                .FirstOrDefaultAsync(m => m.RoomId == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // GET: Rooms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoomId,Name,Description,Capacity,Equipment,PricePerHour,StarRating,GuestRating,NumberOfRooms,IsAvailable")] Room room)
        {
            if (ModelState.IsValid)
            {
                _context.Add(room);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(room);
        }

        // GET: Rooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Room.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            return View(room);
        }

        // POST: Rooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RoomId,Name,Description,Capacity,Equipment,PricePerHour,StarRating,GuestRating,NumberOfRooms,IsAvailable")] Room room)
        {
            if (id != room.RoomId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(room);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.RoomId))
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
            return View(room);
        }

        // GET: Rooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Room
                .FirstOrDefaultAsync(m => m.RoomId == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var room = await _context.Room.FindAsync(id);
            if (room != null)
            {
                _context.Room.Remove(room);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomExists(int id)
        {
            return _context.Room.Any(e => e.RoomId == id);
        }

        public async Task<IActionResult> FilterByRooms(int rooms)
        {
            var filteredRooms = await _context.Room.Where(h => h.NumberOfRooms == rooms).ToListAsync();
            return View("Index", filteredRooms);
        }

        public async Task<IActionResult> AvailableRooms()
        {
            var availableRooms = await _context.Room.Where(h => h.IsAvailable == true).ToListAsync();
            return View("Index", availableRooms);
        }

        public async Task<IActionResult> SortByGuestRating(bool ascending)
        {
            if (ascending == true)
            {
                var sortedRooms = await _context.Room.OrderBy(h => h.GuestRating).ToListAsync();
                return View("Index", sortedRooms);
            }
            else if (ascending == false)
            {
                var sortedRooms = await _context.Room.OrderByDescending(h => h.GuestRating).ToListAsync();
                return View("Index", sortedRooms);
            }
            return View("Index");
        }

        public async Task<IActionResult> FilterByPrice(double minPrice, double maxPrice)
        {
            var filteredRooms = await _context.Room.Where(h => h.PricePerHour >= minPrice && h.PricePerHour <= maxPrice).ToListAsync();
            return View("Index", filteredRooms);
        }
    }
}
