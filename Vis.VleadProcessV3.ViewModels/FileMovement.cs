namespace Vis.VleadProcessV3.ViewModels
{
    public class FileMovement
    {
        public int OrderId { get; set; }
        public int IsClientOrder { get; set; }
        public int? ProcessId { get; set; }
        public int StatusId { get; set; }
        public string SourcePath { get; set; }
        public string DynamicFolderPath { get; set; }
        public string FolderPath { get; set; }
        public string FileName { get; set; }
        public int FileCount { get; set; }
        public int WFMId { get; set; }
        public int WFTId { get; set; }
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
        public string Message { get; set; }
        public string CreditMessage { get; set; }
        public string ClientName { get; set; }
        public int ClientId { get; set; }
    }
}
