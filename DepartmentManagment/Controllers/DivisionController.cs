using DepartmentManagment.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;
using DepartmentManagment.Models.ViewModels;
using DepartmentManagment.Models;
using System.Collections.Generic;

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
            /*
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
            */

            var model = new EmployeeViewModel
            {
                DivisionEmployeeList = await _db.DivisionEmployeesModel.Include(x => x.Employee).Include(x => x.Division.Department)
                .Include(x => x.Division).Where(x => x.Division_Id == id).ToListAsync(),

                DivisionEmployees = new DivisionEmployee()
                {
                    Division_Id = id
                },

                Division = await _db.Divisions.FirstOrDefaultAsync(x => x.Id == id)
            };

            List<int> tempAuthorsAssignedList = model.DivisionEmployeeList.Select(x => x.Employee_Id).ToList();

            // Get all items who's Id isn't in tempAuthorsAssignedList and tempCitiesAssignedList
            var tempEmployeesList = await _db.Employees.Where(x => !tempAuthorsAssignedList.Contains(x.Id)).ToListAsync();

            model.DivisionEmployeeListDropDown = tempEmployeesList.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });

            return View(model);
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

        // Many to Many Relationship methods
        public async Task<IActionResult> ManageEmployees(int id)
        {
            var model = new EmployeeViewModel
            {
                DivisionEmployeeList = await _db.DivisionEmployeesModel.Include(x => x.Employee)
                .Include(x => x.Division).Where(x => x.Division_Id == id).ToListAsync(),

                DivisionEmployees = new DivisionEmployee()
                {
                    Division_Id = id
                },

                Division = await _db.Divisions.FirstOrDefaultAsync(x => x.Id == id)
            };

            List<int> tempAuthorsAssignedList = model.DivisionEmployeeList.Select(x => x.Employee_Id).ToList();

            // Get all items who's Id isn't in tempAuthorsAssignedList and tempCitiesAssignedList
            var tempEmployeesList = await _db.Employees.Where(x => !tempAuthorsAssignedList.Contains(x.Id)).ToListAsync();

            model.DivisionEmployeeListDropDown = tempEmployeesList.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageEmployees(EmployeeViewModel model)
        {
            if (model.DivisionEmployees.Division_Id != 0 && model.DivisionEmployees.Employee_Id != 0)
            {
                _db.DivisionEmployeesModel.Add(model.DivisionEmployees);
                await _db.SaveChangesAsync();
            }

            return RedirectToAction(nameof(ManageEmployees), new { @id = model.DivisionEmployees.Division_Id });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveEmployees(int id, EmployeeViewModel model)
        {
            int newsId = model.Division.Id;
            var employees = await _db.DivisionEmployeesModel.FirstOrDefaultAsync(x => x.Employee_Id == id && x.Division_Id == newsId);

            _db.DivisionEmployeesModel.Remove(employees);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(ManageEmployees), new { @id = newsId });
        }
    }
}
