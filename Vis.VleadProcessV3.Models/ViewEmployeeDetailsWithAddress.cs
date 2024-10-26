using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ViewEmployeeDetailsWithAddress
{
    public int? EmployeeId { get; set; }

    public string? EmployeeName { get; set; }

    public string? EmployeeCode { get; set; }

    public int? CompanyId { get; set; }

    public int? DepartmentId { get; set; }

    public int? DesignationId { get; set; }

    public int? ProfiencyId { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public DateTime? DateOfJoining { get; set; }

    public DateTime? DateOfResignation { get; set; }

    public int? ResignReasons { get; set; }

    public string? Email { get; set; }

    public string? MaritalStatus { get; set; }

    public string? Gender { get; set; }

    public string? PhoneNo { get; set; }

    public string? MobileNo { get; set; }

    public string? BloodGroup { get; set; }

    public string? EmergencyContactName { get; set; }

    public string? EmergencyContactNo { get; set; }

    public bool? IsDeleted { get; set; }

    public int? UpdatedBy { get; set; }

    public int? Id { get; set; }

    public string? DepartmentDesc { get; set; }

    public int? DesigId { get; set; }

    public string? DesignationDesc { get; set; }

    public int? ProfId { get; set; }

    public string? ProfiencyDesc { get; set; }

    public int? ReportingManager1 { get; set; }

    public string? ManagerName1 { get; set; }

    public int? ReportLeader1 { get; set; }

    public string? LeaderName1 { get; set; }

    public int? ReportingManager2 { get; set; }

    public string? ManagerName2 { get; set; }

    public int? ReportingLeader2 { get; set; }

    public string? LeaderName2 { get; set; }

    public string Address1 { get; set; } = null!;

    public string? Address2 { get; set; }

    public string? Address3 { get; set; }

    public string Address11 { get; set; } = null!;

    public string? Address22 { get; set; }

    public string? Address33 { get; set; }

    public string AddressType { get; set; } = null!;

    public int LocationId { get; set; }

    public int? ProcessId { get; set; }

    public long? Row { get; set; }
}
