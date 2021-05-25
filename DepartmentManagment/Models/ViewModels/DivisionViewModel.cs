using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace DepartmentManagment.Models.ViewModels
{
    public class DivisionViewModel
    {
        public Division Division { get; set; }

        // SelectListItem need for DropDown.
        public IEnumerable<SelectListItem> DepartmentList { get; set; }
    }
}
