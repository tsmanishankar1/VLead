using Vis.VleadProcessV3.Models;

namespace Vis.VleadProcessV3.ViewModels
{
    public class EmployeeViewModel
    {
    }
    public class EvsDDDPara
    {
        public int Delete { get; set; }
    }
    public class EvsDPara
    {
        public IEnumerable<Employee> SelectedEmployee { get; set; }
        public IEnumerable<Division> SelectedDivision { get; set; }
        public int CreatedBy { get; set; }
        public IEnumerable<EvsDPara> EDPara { get; set; }
    }


    //-----------------------------------------------------------------ESs------------------------------------------------------------------------------------------
    public class ESS
    {
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public int DivisionId { get; set; }
        public string WorkingStatus { get; set; }
        public int SkillsetId { get; set; }
        public string ProficiencyLevel { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public IEnumerable<ESS> AddCountpara { get; set; }
    }
}



public class EvsDPara1
{
    public Selectedemployee[] SelectedEmployee { get; set; }
    public Selecteddivision[] SelectedDivision { get; set; }
    public int CreatedBy { get; set; }
}

public class Selectedemployee
{
    public int EmployeeId { get; set; }
    public int DepartmentId { get; set; }
}

public class Selecteddivision
{
    public int Id { get; set; }
}
