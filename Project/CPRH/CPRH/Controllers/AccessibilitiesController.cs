using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CPRH.Data;
using CPRH.Models;

namespace CPRH.Controllers
{
    public class AccessibilitiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccessibilitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Accessibilities
        public async Task<IActionResult> Index()
        {
            return View(await _context.Accessibility.ToListAsync());
        }

        // GET: Accessibilities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accessibility = await _context.Accessibility
                .FirstOrDefaultAsync(m => m.AccessibilityId == id);
            if (accessibility == null)
            {
                return NotFound();
            }

            return View(accessibility);
        }

        // GET: Accessibilities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Accessibilities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccessibilityId,FeatureName,Description")] Accessibility accessibility)
        {
            if (ModelState.IsValid)
            {
                _context.Add(accessibility);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(accessibility);
        }

        // GET: Accessibilities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accessibility = await _context.Accessibility.FindAsync(id);
            if (accessibility == null)
            {
                return NotFound();
            }
            return View(accessibility);
        }

        // POST: Accessibilities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AccessibilityId,FeatureName,Description")] Accessibility accessibility)
        {
            if (id != accessibility.AccessibilityId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(accessibility);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccessibilityExists(accessibility.AccessibilityId))
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
            return View(accessibility);
        }

        // GET: Accessibilities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accessibility = await _context.Accessibility
                .FirstOrDefaultAsync(m => m.AccessibilityId == id);
            if (accessibility == null)
            {
                return NotFound();
            }

            return View(accessibility);
        }

        // POST: Accessibilities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var accessibility = await _context.Accessibility.FindAsync(id);
            if (accessibility != null)
            {
                _context.Accessibility.Remove(accessibility);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccessibilityExists(int id)
        {
            return _context.Accessibility.Any(e => e.AccessibilityId == id);
        }
    }
}
