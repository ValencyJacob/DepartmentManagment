using DepartmentManagment.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;
using DepartmentManagment.Models.ViewModels;

namespace DepartmentManagment.Controllers
{
    public class DivisionController : Controller
    {
        private readonly ApplicationDbContext _db;

        public DivisionController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var models = await _db.Divisions.Include(x => x.Department)
                .ToListAsync();

            return View(models);
        }

        [HttpGet]
        public async Task<IActionResult> Upsert(int? id)
        {
            var model = new DivisionViewModel();

            // SelectListUtem for DropDown. Logic locate in App.Models/Models/ViewModels/NewsViewModel
            model.DepartmentList = _db.Departments.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });

            // Create
            if (id == null)
            {
                return View(model);
            }

            // Edit
            model.Division = await _db.Divisions.FirstOrDefaultAsync(x => x.Id == id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(DivisionViewModel model)
        {
            if (model.Division.Id == 0)
            {
                // Create
                await _db.Divisions.AddAsync(model.Division);
            }
            else
            {
                // Update
                _db.Divisions.Update(model.Division);
            }

            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var model = await _db.Divisions.Include(x => x.Department)
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

        public async Task<IActionResult> Delete(int id)
        {
            var model = await _db.Divisions.FirstOrDefaultAsync(x => x.Id == id);

            if (model != null)
            {
                _db.Divisions.Remove(model);
                await _db.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}
