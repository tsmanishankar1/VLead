using Vis.VleadProcessV3.Models;

namespace Vis.VleadProcessV3.ViewModels
{
    public class DropdownCollection
    {
        public IEnumerable<Department> DepartmentList { get; set; }
        public IEnumerable<Designation> DesignationList { get; set; }
        public IEnumerable<Company> CompanyList { get; set; }
        public IEnumerable<Proficiency> ProficiencyList { get; set; }
        public IEnumerable<Division> DivisionList { get; set; }

    }
}
