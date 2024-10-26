using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class TATReport_Result
    {
        public string? Description { get; set; }
        public string? ClientName { get; set; }
        public Nullable<int> ClientId { get; set; }
        public int DepartmentId { get; set; }
        public string? CustomerJobType { get; set; }
        public Nullable<int> Total_Files_Recd { get; set; }
        public Nullable<int> Artist_Pending { get; set; }
        public Nullable<int> Artist_Completed { get; set; }
        public Nullable<int> Quality_Pending { get; set; }
        public Nullable<int> Quality_Completed { get; set; }
        public Nullable<int> Proof_Reading_Pending { get; set; }
        public Nullable<int> Proof_Reading_Completed { get; set; }
        public Nullable<int> Uploaded_Files { get; set; }
        public Nullable<int> Buddy_Proof { get; set; }
        public Nullable<int> Proof_Correction { get; set; }
        public Nullable<int> Support_Pending { get; set; }
        public Nullable<int> Hold { get; set; }
    }
    [Keyless]
    public partial class TATReport_Vlead_Result
    {
        public string? Description { get; set; }
        public string? ClientName { get; set; }
        public Nullable<int> ClientId { get; set; }
        public int DepartmentId { get; set; }
        public string? CustomerJobType { get; set; }
        public Nullable<int> Total_Files_Recd { get; set; }
        public Nullable<int> Artist_Pending { get; set; }
        public Nullable<int> Artist_Completed { get; set; }
        public Nullable<int> Quality_Pending { get; set; }
        public Nullable<int> Quality_Completed { get; set; }
        public Nullable<int> Proof_Reading_Pending { get; set; }
        public Nullable<int> Proof_Reading_Completed { get; set; }
        public Nullable<int> Uploaded_Files { get; set; }
        public Nullable<int> Buddy_Proof { get; set; }
        public Nullable<int> Proof_Correction { get; set; }
        public Nullable<int> Support_Pending { get; set; }
        public Nullable<int> Hold { get; set; }
    }
}
