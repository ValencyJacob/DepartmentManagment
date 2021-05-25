using DepartmentManagment.Data;
using DepartmentManagment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DepartmentManagment.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext _db;

        public DepartmentController(ApplicationDbContext db)
        {
            _db = db;
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

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Department model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id == 0)
                {
                    // Create
                    await _db.Departments.AddAsync(model);
                }
                else
                {
                    // Update
                    _db.Departments.Update(model);
                }

                await _db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var model = await _db.Departments.FirstOrDefaultAsync(x => x.Id == id);

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
