using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CheckOutForm.Models;
using Microsoft.Data.SqlClient;
using System.IO;

namespace CheckOutForm.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class RentalsController : Controller
    {
        private readonly CheckOutFormContext _context;

        public RentalsController(CheckOutFormContext context)
        {
            _context = context;
        }

        // GET: Rentals
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["RenterNameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["BeginDateSortParm"] = sortOrder == "BeginDate" ? "begindate_desc" : "BeginDate";
            ViewData["EndDateSortParm"] = sortOrder == "EndDate" ? "enddate_desc" : "EndDate";
            ViewData["ApprovedBySortParm"] = String.IsNullOrEmpty(sortOrder) ? "approveby_desc" : "";
            ViewData["InfoSortParm"] = String.IsNullOrEmpty(sortOrder) ? "info_desc" : "";
            ViewData["DeviceSortParm"] = String.IsNullOrEmpty(sortOrder) ? "device_desc" : "";
            ViewData["StatusSortParm"] = String.IsNullOrEmpty(sortOrder) ? "status_desc" : "";
            ViewData["CurrentFilter"] = searchString;

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var rentals = from r in _context.Rentals.Include(r => r.CurrentStatus).AsNoTracking()
                          select r;
            if (!String.IsNullOrEmpty(searchString))
            {
                rentals = rentals.Where(r => r.RenterName.Contains(searchString)
                                       || r.BeginRental.ToString().Contains(searchString) || r.EndRental.ToString().Contains(searchString));
            }
            rentals = sortOrder switch
            {
                "name_desc" => rentals.OrderBy(r => r.RenterName),
                "BeginRental" => rentals.OrderBy(r => r.BeginRental),
                "begindate_desc" => rentals.OrderByDescending(r => r.BeginRental),
                "EndRental" => rentals.OrderBy(r => r.EndRental),
                "enddate_desc" => rentals.OrderByDescending(r => r.EndRental),
                "approveby_desc" => rentals.OrderByDescending(r => r.ApprovedBy),
                "info_desc" => rentals.OrderByDescending(r => r.Info),
                "device_desc" => rentals.OrderByDescending(r => r.Devices.NameTag),
                "status_desc" => rentals.OrderByDescending(r => r.CurrentStatus.ApprovalStatus),
                _ => rentals.OrderByDescending(r => r.BeginRental),
            };
            int pageSize = 10;
            return View(await PaginatedList<Rentals>.CreateAsync(rentals
                .Include(i => i.Devices)
                .Include(i => i.CurrentStatus)
                .AsNoTracking(), pageNumber ?? 1, pageSize));
        }


        // GET: Rentals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentals = await _context.Rentals
                .Include(i => i.Devices)
                .Include(i => i.CurrentStatus)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.RentalsID == id);
            if (rentals == null)
            {
                return NotFound();
            }
            return View(rentals);
        }

        // GET: Rentals/Create
        public IActionResult Create()
        {
            PopulateDeviceDropDownList();
            PopulateCurrentStatusDropDownList();
            return View();
        }

        // POST: Rentals/Create
        // Enable the specific properties you want to bind to, for protecting against overposting attacks
        // Reference:http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RentalsID,RenterName,BeginRental,EndRental,ApprovedBy,Info,DevicesID,CurrentStatusID")] Rentals rentals)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(rentals);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            PopulateDeviceDropDownList(rentals.DevicesID);
            PopulateCurrentStatusDropDownList(rentals.CurrentStatusID);
            return View(rentals);
        }

        // GET: Rentals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentals = await _context.Rentals.AsNoTracking().FirstOrDefaultAsync(r => r.RentalsID == id);
            if (rentals == null)
            {
                return NotFound();
            }
            PopulateDeviceDropDownList(rentals.DevicesID);
            PopulateCurrentStatusDropDownList(rentals.CurrentStatusID);
            return View(rentals);
        }

        // POST: Rentals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var rentalsToUpdate = await _context.Rentals.FirstOrDefaultAsync(r => r.RentalsID == id);
            if (await TryUpdateModelAsync(rentalsToUpdate,
                "",
                r => r.RenterName, r => r.BeginRental, r => r.EndRental, r => r.ApprovedBy, r => r.Info, r => r.DevicesID, r => r.CurrentStatusID))
            {
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
                return RedirectToAction(nameof(Index));
            }
            PopulateDeviceDropDownList(rentalsToUpdate.DevicesID);
            PopulateCurrentStatusDropDownList(rentalsToUpdate.CurrentStatusID);
            return View(rentalsToUpdate);
        }
        private void PopulateDeviceDropDownList(object selectedDevice = null)
        {
            var deviceQuery = from d in _context.Devices
                              where d.Assignee == "Loaner" && d.Retired != "True"
                              orderby d.DevicesID
                              select d;
            ViewBag.DevicesID = new SelectList(deviceQuery.AsNoTracking(), "DevicesID", "NameTag", selectedDevice);
        }

        private void PopulateCurrentStatusDropDownList(object selectedCurrentStatus = null)
        {
            var currentStatusQuery = from cs in _context.CurrentStatus
                                     orderby cs.CurrentStatusID
                                     select cs;
            ViewBag.CurrentStatusID = new SelectList(currentStatusQuery.AsNoTracking(), "CurrentStatusID", "ApprovalStatus", selectedCurrentStatus);
        }

        // GET: Rentals/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentals = await _context.Rentals
                .Include(r => r.CurrentStatus)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.RentalsID == id);
            if (rentals == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }
            return View(rentals);
        }

        // POST: Rentals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rentals = await _context.Rentals.FindAsync(id);
            if (rentals == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Rentals.Remove(rentals);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { id, saveChangesError = true });
            }
        }
        public void DumpTableToFile()
        {
            SqlConnection sqlCon = new SqlConnection("Server = SqlServer\\SQLDBInstanceName; Database = SQLDB_Name; Trusted_Connection = True; MultipleActiveResultSets = true");
            sqlCon.Open();

            SqlCommand sqlCmd = new SqlCommand(
                "DECLARE @current_year DATE = GETDATE() USE SQLDB_Name; SELECT * FROM dbo.TableName WHERE Year(BeginRental) >= Year(@current_year)", sqlCon);
            SqlDataReader reader = sqlCmd.ExecuteReader();

            string fileName = "Rental_Report.csv";
            StreamWriter sw = new StreamWriter(fileName);
            object[] output = new object[reader.FieldCount];

            for (int i = 0; i < reader.FieldCount; i++)
                output[i] = reader.GetName(i);

            sw.WriteLine(string.Join(",", output));

            while (reader.Read())
            {
                reader.GetValues(output);
                sw.WriteLine(string.Join(",", output));
            }

            sw.Close();
            reader.Close();
            sqlCon.Close();
            sqlCmd.Dispose();
            Response.Redirect("Exported");
        }
        // GET: Rentals/Exported
        public IActionResult Exported()
        {
            return View();
        }
    }
}
