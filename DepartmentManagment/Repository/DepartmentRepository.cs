using DepartmentManagment.Data;
using DepartmentManagment.Models;
using DepartmentManagment.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DepartmentManagment.Repository
{
    public class DepartmentRepository : Repository<Department>, IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;

        public DepartmentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task UpdateAsync(Department item)
        {
            var model = await _context.Departments.FirstOrDefaultAsync(x => x.Id == item.Id);

            if (model != null)
            {
                if (item.ImageUrl != null)
                {
                    model.ImageUrl = item.ImageUrl;
                }

                model.Name = item.Name;
                model.Description = item.Description;
                model.Divisions = item.Divisions;

            }
        }
    }
}
