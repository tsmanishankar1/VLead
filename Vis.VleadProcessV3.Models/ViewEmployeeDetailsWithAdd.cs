using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ViewEmployeeDetailsWithAdd
{
    public int EmployeeId { get; set; }

    public string EmployeeName { get; set; } = null!;

    public string EmployeeCode { get; set; } = null!;

    public int CompanyId { get; set; }
    
    public int CreatedBy { get; set; }

    public int DepartmentId { get; set; }

    public int DesignationId { get; set; }

    public int ProfiencyId { get; set; }

    public DateTime DateOfBirth { get; set; }

    public DateTime DateOfJoining { get; set; }

    public DateTime? DateOfResignation { get; set; }

    public int? ResignReasons { get; set; }

    public string? Email { get; set; }

    public string? PersonalEmail { get; set; }

    public string MaritalStatus { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public string? PhoneNo { get; set; }

    public string MobileNo { get; set; } = null!;

    public string BloodGroup { get; set; } = null!;

    public string EmergencyContactName { get; set; } = null!;

    public string EmergencyContactNo { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public bool IsOutsource { get; set; }

    public int? UpdatedBy { get; set; }

    public int Id { get; set; }

    public string DepartmentDesc { get; set; } = null!;

    public int DesigId { get; set; }

    public string DesignationDesc { get; set; } = null!;

    public int ProfId { get; set; }

    public string ProfiencyDesc { get; set; } = null!;

    public int? ReportingManager1 { get; set; }

    public int? ReportLeader1 { get; set; }

    public int? ReportingManager2 { get; set; }

    public string? ManagerName1 { get; set; }

    public int? ReportingLeader2 { get; set; }

    public string? LeaderName2 { get; set; }

    public string? Address1 { get; set; }

    public string? Address2 { get; set; }

    public string? Address3 { get; set; }

    public string? Address11 { get; set; }

    public string? Address22 { get; set; }

    public string? Address33 { get; set; }

    public string? AddressType { get; set; }

    public int? LocationId { get; set; }

    public int? ProcessId { get; set; }

    public string? IsInternetConnection { get; set; }

    public string? IsSystem { get; set; }

    public string? NetWorkType { get; set; }

    public string? ServiceProvider { get; set; }

    public string? SystemConfig { get; set; }
    public string? WorkingStatus { get; set; }
    public int CoreDivision { get; set; }
    public string? BatchNo { get; set; }
}
