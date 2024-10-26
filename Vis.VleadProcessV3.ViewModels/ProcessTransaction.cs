namespace Vis.VleadProcessV3.ViewModels
{
    public class ProcessTransaction
    {
        public int Wftid { get; set; }
        public int Wfmid { get; set; }
        public string WorkType { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public int EmployeeId { get; set; }
        public bool CopyFiles { get; set; }
        public int? ErrorCategoryId { get; set; }
        //public decimal Value { get; set; }
        public int Value { get; set; }
        public int? ScopeId { get; set; }
        public int ProcessId { get; set; }
        public long StitchCount { get; set; }

        //filemovement
        public int OrderId { get; set; }
        public int IsClientOrder { get; set; }
        public int StatusId { get; set; }
        public string SourcePath { get; set; }
        public string DynamicFolderPath { get; set; }
        public string FolderPath { get; set; }
        public string FileName { get; set; }
        public int FileCount { get; set; }
        public string OrignalPath { get; set; }
        public string OrignalDynamicPath { get; set; }
        public string JobId { get; set; }
        public int IsProcessWorkFlowTranInserted { get; set; }
        public int IsCopyFiles { get; set; }
        public int? Pid { get; set; }
        public int? FakeProcessId { get; set; }
        public int FakeStatusId { get; set; }
        public string FakeDynamicFolderPath { get; set; }
        public string JobFileName { get; set; }
        public List<string> files { get; set; }
        public string CommentsToClient { get; set; }

        public string TranFileUploadPath { get; set; }
        public IEnumerable<ProcessTransaction> SelectedRows { get; set; }
    }
}
