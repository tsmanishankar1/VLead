using Microsoft.Extensions.FileProviders;

namespace VisProcess
{
    public class WebHostEnvironment : IWebHostEnvironment
    {
        public WebHostEnvironment()
        {
                ContentRootPath=Directory.GetCurrentDirectory();
                ApplicationName = "VisProcess";
                ContentRootFileProvider = new PhysicalFileProvider(ContentRootPath);
        }
        public string EnvironmentName { get; set; }
        public string ApplicationName { get; set; }
        public string WebRootPath { get; set; }
        public IFileProvider WebRootFileProvider { get; set; }
        public string ContentRootPath { get; set; }
        public IFileProvider ContentRootFileProvider { get; set; }

    }
}
