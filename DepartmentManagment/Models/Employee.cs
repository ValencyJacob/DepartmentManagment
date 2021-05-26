using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DepartmentManagment.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int RoleId { get; set; }
        public EmployeeRole Role { get; set; }
    }
}
