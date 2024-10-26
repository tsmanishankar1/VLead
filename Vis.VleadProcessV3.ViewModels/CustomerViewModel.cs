namespace Vis.VleadProcessV3.ViewModels
{
    public class CustomerViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
    }
    public class CvsDPara
    {
        public int CustomerId { get; set; }
        public int DepartmentId { get; set; }
        public int DivisionId { get; set; }
        public int EmployeeId { get; set; }
    }
    //------------------------------------final CL-----------------------------------------------------
    public class CvsCPara
    {
        public int CustomerId { get; set; }
        public int DepartmentId { get; set; }
        public string Description { get; set; }
        public int EmployeeId { get; set; }
    }
    //------------------------------------final CL-----------------------------------------------------
    //-------------------------------------vidhya KRA---------------------------------------------------
    public class KRA
    {
        public int EmployeeId { get; set; }
    }
    //-------------------------------------vidhya KRA---------------------------------------------------
    //-------------------------------------wellness status---------------------------------------------------
    public class WELL
    {
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string EMobile { get; set; }
        public string EMail { get; set; }
        public string Stay { get; set; }
        public string Transport { get; set; }
        public string Aarogya { get; set; }
        public string Covid { get; set; }
        public string Foreigns { get; set; }
        public string UForeign { get; set; }
        public string Contact { get; set; }
        public string Area { get; set; }
        public string ContactF { get; set; }
        public string Symptoms { get; set; }
        public string Family { get; set; }
    }
}
