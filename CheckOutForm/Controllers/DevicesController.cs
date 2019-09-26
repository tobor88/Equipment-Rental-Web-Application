using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CheckOutForm.Models;
using CheckOutForm.Models.DataGroups;

namespace CheckOutForm.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class DevicesController : Controller
    {
        public DateTime TodaysDate { get; private set; } = DateTime.UtcNow;   // Selects todays date based on client location instead of server location

        private readonly CheckOutFormContext _context;
        public DevicesController(CheckOutFormContext context)
        {
            _context = context;
        }

        // Summarize the count of devices in the database
        public async Task<ActionResult> Summary()
        {
            IQueryable<DeviceTypeGroup>
                 data =
                 from device in _context.Devices
                 group device by device.DeviceType into deviceTypeGroup
                 select new DeviceTypeGroup()
                 {
                     DeviceTypes = deviceTypeGroup.Key,
                     DeviceTypesCount = deviceTypeGroup.Count(),
                     RetiredCount = (deviceTypeGroup.Where(dc => dc.Retired == "True")).Count(),
                     AvailableLoaners = (deviceTypeGroup.Where(dc => dc.Assignee == "Loaner")).Count(),

                     ADevices = (deviceTypeGroup.Where(dc => dc.NameTag.Contains("A-") || dc.NameTag.Contains("USAV-Tablet")).Count(),
                     BDevices = (deviceTypeGroup.Where(dc => dc.NameTag.Contains("B-")).Count(),
                     CDevices = (deviceTypeGroup.Where(dc => dc.NameTag.Contains("C-") || dc.NameTag.Contains("Tab") || dc.NameTag.Contains("iPad"))).Count(),
                     DDevices = (deviceTypeGroup.Where(dc => dc.NameTag.Contains("D-"))).Count(),

                     ARetired = (deviceTypeGroup.Where(dc => dc.NameTag.Contains("A-") || dc.NameTag.Contains("A-Tablet") || dc.NameTag.Contains("ITTab") || dc.NameTag.Contains("Projector".Count(),
                     BRetired = (deviceTypeGroup.Where(dc => dc.NameTag.Contains("B-") && dc.Retired == "True")).Count(),
                     CRetired = (deviceTypeGroup.Where(dc => dc.NameTag.Contains("C-") || dc.NameTag.Contains("Tab") || dc.NameTag.Contains("iPad")).Where(b => b.Retired == "True")).Count(),
                     DRetired = (deviceTypeGroup.Where(dc => dc.NameTag.Contains("D-") && dc.Retired == "True")).Count(),
                 };
            return View(await data.AsNoTracking().ToListAsync());
        }

        public async Task<IActionResult> Index(int? devicesID, string sortOrder, string currentFilter, string searchString, int? pageNumber) // Removed  int? deviceSpecsID,
        {
            var viewModel = new DeviceIndexData
            {
                Devices = await _context.Devices
                  .AsNoTracking()
                  .OrderBy(i => i.DevicesID)
                  .ToListAsync()
            };

            if (devicesID != null)
            {
                ViewData["DevicesID"] = devicesID.Value;
                Devices deviceModels = viewModel.Devices.Where(i => i.DevicesID == devicesID.Value).Single();
            }

            ViewData["CurrentSort"] = sortOrder;
            ViewData["DeviceTypeSortParam"] = String.IsNullOrEmpty(sortOrder) ? "type_desc" : "";
            ViewData["NameTagSortParam"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["AssigneeSortParam"] = String.IsNullOrEmpty(sortOrder) ? "assigne_desc" : "";
            ViewData["DomainSortParam"] = String.IsNullOrEmpty(sortOrder) ? "domain_desc" : "";
            ViewData["EntryDateSortParam"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["BrandSortParam"] = String.IsNullOrEmpty(sortOrder) ? "brand_desc" : "";
            ViewData["ModelSortParam"] = String.IsNullOrEmpty(sortOrder) ? "model_desc" : "";
            ViewData["SerialSortParam"] = String.IsNullOrEmpty(sortOrder) ? "serial_desc" : "";
            ViewData["OSSortParam"] = String.IsNullOrEmpty(sortOrder) ? "os_desc" : "";
            ViewData["CPUSortParam"] = String.IsNullOrEmpty(sortOrder) ? "cpu_desc" : "";
            ViewData["CPUSpeedSortParam"] = String.IsNullOrEmpty(sortOrder) ? "cpuspeed_desc" : "";
            ViewData["RAMSortParam"] = String.IsNullOrEmpty(sortOrder) ? "ram_desc" : "";
            ViewData["HDSizeSortParam"] = String.IsNullOrEmpty(sortOrder) ? "hdsize_desc" : "";
            ViewData["HDTypeSortParam"] = String.IsNullOrEmpty(sortOrder) ? "hdtype_desc" : "";
            ViewData["OpticalSortParam"] = String.IsNullOrEmpty(sortOrder) ? "optical_desc" : "";
            ViewData["ScreenSizeSortParam"] = String.IsNullOrEmpty(sortOrder) ? "screen_desc" : "";
            ViewData["UserSortParam"] = String.IsNullOrEmpty(sortOrder) ? "user_desc" : "";
            ViewData["PasswordSortParam"] = String.IsNullOrEmpty(sortOrder) ? "pwd_desc" : "";
            ViewData["RetiredSortParam"] = String.IsNullOrEmpty(sortOrder) ? "retired_desc" : "";
            ViewData["NotesSortParam"] = String.IsNullOrEmpty(sortOrder) ? "notes_desc" : "";
            ViewData["CurrentFilter"] = searchString;

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var devices = from d in _context.Devices
                          select d;
            if (!String.IsNullOrEmpty(searchString))
            {
                devices = devices.Where(s => s.NameTag.Contains(searchString) || s.Assignee.Contains(searchString) || s.EntryDate.ToString().Contains(searchString) || s.DeviceType.Contains(searchString) || s.Notes.Contains(searchString) || s.Model.Contains(searchString));
            }
            devices = sortOrder switch
            {
                "type_desc" => devices.OrderBy(s => s.DeviceType),
                "name_desc" => devices.OrderBy(s => s.NameTag),
                "assigne_desc" => devices.OrderBy(s => s.Assignee),
                "domain_desc" => devices.OrderBy(s => s.Domain),
                "Date" => devices.OrderBy(s => s.EntryDate),
                "date_desc" => devices.OrderBy(s => s.EntryDate),
                "brand_desc" => devices.OrderBy(s => s.Brand),
                "model_desc" => devices.OrderBy(s => s.Model),
                "serial_desc" => devices.OrderBy(s => s.Serial),
                "os_desc" => devices.OrderBy(s => s.OS),
                "cpu_desc" => devices.OrderBy(s => s.CPU),
                "cpuspeed_desc" => devices.OrderBy(s => s.ProcessorSpeed),
                "ram_desc" => devices.OrderBy(s => s.RAM),
                "hdsize_desc" => devices.OrderBy(s => s.DriveSize),
                "hdtype_desc" => devices.OrderBy(s => s.HardDriveType),
                "optical_desc" => devices.OrderBy(s => s.Optical),
                "screen_desc" => devices.OrderBy(s => s.ScreenSize),
                "user_desc" => devices.OrderBy(s => s.User),
                "pwd_desc" => devices.OrderBy(s => s.Pwd),
                "retired_desc" => devices.OrderBy(s => s.Retired),
                "notes_desc" => devices.OrderBy(s => s.Notes),
                _ => devices.OrderBy(s => s.DevicesID),
            };
            int pageSize = 10;
            return View(await PaginatedList<Devices>.CreateAsync(devices.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Devices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var devices = await _context.Devices
                .FirstOrDefaultAsync(m => m.DevicesID == id);
            if (devices == null)
            {
                return NotFound();
            }

            return View(devices);
        }

        // GET: Devices/Create
        public IActionResult Create()
        {
            return View(new Devices
            {
                DeviceType = "Laptop",
                NameTag = "A-",
                Assignee = "User",
                Brand = "Lenovo",
                Model = "Yoga 720-13IKB 81CS",
                Serial = "S/N:",
                OS = "Windows 10 Pro x64",
                CPU = "Intel i5-8250U",
                ProcessorSpeed = "1.6GHz",
                RAM = "8GB",
                DriveSize = "250GB",
                HardDriveType = "SSD",
                Optical = "No",
                ScreenSize = 13,
                User = "tobor",
                Pwd = "I_Am_God@legion",
                Domain = "osbornepro.com",
                EntryDate = TodaysDate,
                Retired = "False",
                Notes = "None"
            }
                );
        }

        // POST: Devices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DeviceType,NameTag,Assignee,Domain,EntryDate,Brand,Model,Serial,OS,CPU,ProcessorSpeed,RAM,DriveSize,HardDriveType,Optical,ScreenSize,User,Pwd,Retired,Notes")] Devices devices) // removed , string[] selectedDeviceSpecs from the end
        {
            if (ModelState.IsValid)
            {
                _context.Add(devices);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(devices);
        }

        // GET: Devices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var devices = await _context.Devices
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.DevicesID == id);
            if (devices == null)
            {
                return NotFound();
            }
            return View(devices);
        }

        // POST: Devices/Edit/5
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

            var deviceToUpdate = await _context.Devices
                .FirstOrDefaultAsync(r => r.DevicesID == id);

            if (await TryUpdateModelAsync<Devices>(
                deviceToUpdate,
                "",
                d => d.DeviceType, d => d.NameTag, d => d.Assignee, d => d.Domain, d => d.EntryDate, d => d.Brand, d => d.Model, d => d.Serial, d => d.OS, d => d.CPU, d => d.ProcessorSpeed, d => d.RAM, d => d.DriveSize, d => d.HardDriveType, d => d.Optical, d => d.ScreenSize, d => d.User, d => d.Pwd, d => d.Retired, d => d.Notes))
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
            return View(deviceToUpdate);
        }

        // GET: Devices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var devices = await _context.Devices
                .FirstOrDefaultAsync(m => m.DevicesID == id);
            if (devices == null)
            {
                return NotFound();
            }

            return View(devices);
        }

        // POST: Devices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Devices devices = await _context.Devices
                .SingleAsync(i => i.DevicesID == id);

            var currentStatus = await _context.CurrentStatus
                .ToListAsync();

            _context.Devices.Remove(devices);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
