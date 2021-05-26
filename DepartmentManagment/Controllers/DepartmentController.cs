using DepartmentManagment.Data;
using DepartmentManagment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using System;

namespace DepartmentManagment.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHost;
        public DepartmentController(ApplicationDbContext db, IWebHostEnvironment webHost)
        {
            _db = db;
            _webHost = webHost;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var models = await _db.Departments.ToListAsync();

            return View(models);
        }

        [HttpGet]
        public async Task<IActionResult> Upsert(int? id)
        {
            var model = new Department();

            // Create
            if (id == null)
            {
                return View(model);
            }

            // Edit
            model = await _db.Departments.FirstOrDefaultAsync(x => x.Id == id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model); //ImageUrl is not null.
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Department item)
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _webHost.WebRootPath;
                var files = HttpContext.Request.Form.Files;

                if (files.Count > 0)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"images\department");
                    var extenstion = Path.GetExtension(files[0].FileName);

                    if (item.ImageUrl != null) // ImageUrl is null (0_0)
                    {
                        // Update data with image
                        var imagePath = Path.Combine(webRootPath, item.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extenstion), FileMode.Create))
                    {
                        await files[0].CopyToAsync(fileStreams);
                    }

                    item.ImageUrl = @"\images\department\" + fileName + extenstion;
                }
                else
                {
                    // Update data without update image
                    if (item.Id != 0)
                    {
                        var model = await _db.Departments.FindAsync(item.Id);
                        item.ImageUrl = model.ImageUrl;
                    }
                }

                if (item.Id == 0)
                {
                    await _db.Departments.AddAsync(item);
                }
                else
                {
                    _db.Departments.Update(item);
                }

                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                if (item.Id != 0)
                {
                    item = await _db.Departments.FindAsync(item.Id);
                }
            }

            return View(item);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var model = await _db.Departments.FirstOrDefaultAsync(x => x.Id == id);

            string webRootPath = _webHost.WebRootPath;

            var imagePath = Path.Combine(webRootPath, model.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            if (model != null)
            {
                _db.Departments.Remove(model);
                await _db.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var model = await _db.Departments.Include(x => x.Divisions)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (model != null)
            {
                return View(model);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
