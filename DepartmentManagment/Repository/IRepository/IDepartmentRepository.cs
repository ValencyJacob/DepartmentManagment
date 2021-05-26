using DepartmentManagment.Models;
using System.Threading.Tasks;

namespace DepartmentManagment.Repository.IRepository
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        Task UpdateAsync(Department item);
    }
}
