using DepartmentManagment.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;
using DepartmentManagment.Models.ViewModels;
using DepartmentManagment.Models;

namespace DepartmentManagment.Controllers
{
    public class RoleController : Controller
    {
        private readonly ApplicationDbContext _db;

        public RoleController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var models = await _db.EmployeeRoles.ToListAsync();

            return View(models);
        }

        [HttpGet]
        public async Task<IActionResult> Upsert(int? id)
        {
            var model = new EmployeeRole();

            // Create
            if (id == null)
            {
                return View(model);
            }

            // Edit
            model = await _db.EmployeeRoles.FirstOrDefaultAsync(x => x.Id == id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(EmployeeRole model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id == 0)
                {
                    // Create
                    await _db.EmployeeRoles.AddAsync(model);
                }
                else
                {
                    // Update
                    _db.EmployeeRoles.Update(model);
                }

                await _db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var model = await _db.EmployeeRoles.FirstOrDefaultAsync(x => x.Id == id);

            if (model != null)
            {
                _db.EmployeeRoles.Remove(model);

                await _db.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var model = await _db.EmployeeRoles.Include(x => x.Employee)
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
