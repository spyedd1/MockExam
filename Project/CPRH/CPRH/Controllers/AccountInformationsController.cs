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
    public class AccountInformationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountInformationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AccountInformations
        public async Task<IActionResult> Index()
        {
            return View(await _context.AccountInformation.ToListAsync());
        }

        // GET: AccountInformations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accountInformation = await _context.AccountInformation
                .FirstOrDefaultAsync(m => m.AccountInformationId == id);
            if (accountInformation == null)
            {
                return NotFound();
            }

            return View(accountInformation);
        }

        // GET: AccountInformations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AccountInformations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountInformationId,UserId,FullName,PhoneNumber")] AccountInformation accountInformation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(accountInformation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(accountInformation);
        }

        // GET: AccountInformations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accountInformation = await _context.AccountInformation.FindAsync(id);
            if (accountInformation == null)
            {
                return NotFound();
            }
            return View(accountInformation);
        }

        // POST: AccountInformations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AccountInformationId,UserId,FullName,PhoneNumber")] AccountInformation accountInformation)
        {
            if (id != accountInformation.AccountInformationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(accountInformation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountInformationExists(accountInformation.AccountInformationId))
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
            return View(accountInformation);
        }

        // GET: AccountInformations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accountInformation = await _context.AccountInformation
                .FirstOrDefaultAsync(m => m.AccountInformationId == id);
            if (accountInformation == null)
            {
                return NotFound();
            }

            return View(accountInformation);
        }

        // POST: AccountInformations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var accountInformation = await _context.AccountInformation.FindAsync(id);
            if (accountInformation != null)
            {
                _context.AccountInformation.Remove(accountInformation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountInformationExists(int id)
        {
            return _context.AccountInformation.Any(e => e.AccountInformationId == id);
        }
    }
}
