using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DepartmentManagment.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Division> Divisions { get; set; }
    }
}
