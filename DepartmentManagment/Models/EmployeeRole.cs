using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DepartmentManagment.Models
{
    public class EmployeeRole
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int EmployeeId { get; set; }
        
        public List<Employee> Employee { get; set; }
    }
}
