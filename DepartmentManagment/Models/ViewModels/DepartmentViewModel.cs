using System.Collections.Generic;

namespace DepartmentManagment.Models.ViewModels
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        public List<Division> Divisions { get; set; }
    }
}
