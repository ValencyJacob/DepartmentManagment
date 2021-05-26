using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace DepartmentManagment.Models.ViewModels
{
    public class RoleViewModel
    {
        public Employee Employee { get; set; }

        // SelectListItem need for DropDown.
        public IEnumerable<SelectListItem> RoleList { get; set; }
    }
}
