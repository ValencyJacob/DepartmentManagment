using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace DepartmentManagment.Models.ViewModels
{
    public class EmployeeViewModel
    {
        public DivisionEmployee DivisionEmployees { get; set; }
        public Division Division { get; set; }

        public IEnumerable<DivisionEmployee> DivisionEmployeeList { get; set; }

        public IEnumerable<SelectListItem> DivisionEmployeeListDropDown { get; set; }
    }
}
