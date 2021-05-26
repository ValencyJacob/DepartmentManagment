using DepartmentManagment.Data;
using DepartmentManagment.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DepartmentManagment.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public EmployeeController(ApplicationDbContext db)
        {
            _db = db;
        }

        #region Fixed
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var models = await _db.Employees.Include(x => x.Role)
                .ToListAsync();

            return View(models);
        }

        [HttpGet]
        public async Task<IActionResult> Upsert(int? id)
        {
            var model = new RoleViewModel();

            // SelectListUtem for DropDown. Logic locate in App.Models/Models/ViewModels/RoleViewModel
            model.RoleList = _db.EmployeeRoles.Select(x => new SelectListItem
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
            model.Employee = await _db.Employees.FirstOrDefaultAsync(x => x.Id == id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(RoleViewModel model)
        {
            if (model.Employee.Id == 0)
            {
                // Create
                await _db.Employees.AddAsync(model.Employee);
            }
            else
            {
                // Update
                _db.Employees.Update(model.Employee);
            }

            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var model = await _db.Employees.Include(x => x.Role)
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
            var model = await _db.Employees.FirstOrDefaultAsync(x => x.Id == id);

            if (model != null)
            {
                _db.Employees.Remove(model);
                await _db.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
        #endregion
    }
}
