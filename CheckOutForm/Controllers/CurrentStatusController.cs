using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CheckOutForm.Models;

namespace CheckOutForm.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class CurrentStatusController : Controller
    {
        private readonly CheckOutFormContext _context;

        public CurrentStatusController(CheckOutFormContext context)
        {
            _context = context;
        }

        // GET: CurrentStatus
        public async Task<IActionResult> Index()
        {
            var deviceRentalContext = _context.CurrentStatus.Include(c => c.Rentals);
            return View(await deviceRentalContext.ToListAsync());
        }

        // GET: CurrentStatus/Create
        public IActionResult Create()
        {
            ViewData["DevicesID"] = new SelectList(_context.Devices, "DevicesID", "Assignee");
            return View();
        }

        // POST: CurrentStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CurrentStatusID,ApprovalStatus,RentalsID,RowVersion")] CurrentStatus currentStatus)
        {
            if (ModelState.IsValid)
            {
                _context.Add(currentStatus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DevicesID"] = new SelectList(_context.Devices, "DevicesID", "Assignee", currentStatus.RentalsID);
            return View(currentStatus);
        }

        // GET: CurrentStatus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currentStatus = await _context.CurrentStatus.FindAsync(id);
            if (currentStatus == null)
            {
                return NotFound();
            }
            ViewData["DevicesID"] = new SelectList(_context.Devices, "DevicesID", "Assignee", currentStatus.RentalsID);
            return View(currentStatus);
        }

        // POST: CurrentStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CurrentStatusID,ApprovalStatus,RentalsID,RowVersion")] CurrentStatus currentStatus)
        {
            if (id != currentStatus.CurrentStatusID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(currentStatus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CurrentStatusExists(currentStatus.CurrentStatusID))
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
            ViewData["DevicesID"] = new SelectList(_context.Devices, "DevicesID", "Assignee", currentStatus.RentalsID);
            return View(currentStatus);
        }

        // GET: CurrentStatus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currentStatus = await _context.CurrentStatus
                .Include(c => c.Rentals)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.CurrentStatusID == id);
            if (currentStatus == null)
            {
                return NotFound();
            }

            return View(currentStatus);
        }

        // POST: CurrentStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var currentStatus = await _context.CurrentStatus.FindAsync(id);
            _context.CurrentStatus.Remove(currentStatus);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CurrentStatusExists(int id)
        {
            return _context.CurrentStatus.Any(e => e.CurrentStatusID == id);
        }
    }
}