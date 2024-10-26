using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vis.VleadProcessV3.Models;

namespace Vis.VleadProcessV3.Services
{
    public class EmployeeFileUploadService
    {
        private readonly ApplicationDbContext _context;
        private readonly string uploadFile = "Documents";

        public EmployeeFileUploadService(ApplicationDbContext context)
        {
            _context = context;
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), uploadFile);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
        }

        public string EmployeeFileUpload(string employeeCode, string employeeName, EmployeeFileUpload upload)
        {
            try
            {
                var employee = _context.Employees.FirstOrDefault(e => e.EmployeeCode == employeeCode && e.EmployeeName == employeeName && e.IsDeleted == false);
                if (employee == null)
                {
                    throw new MessageNotFoundException("Employee not found.");
                }

                string message = "";

                string formattedDateTime = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
                var fileName = $"{employeeCode}_{employeeName}_{formattedDateTime}{Path.GetExtension(upload.file.FileName)}";
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "uploadFile", fileName);

                var existingFile = _context.EmployeeUploadedFiles.FirstOrDefault(f => f.EmployeeCode == employeeCode && f.EmployeeName == employeeName && f.FilePath == uploadPath && f.IsDeleted == false);
                if (existingFile != null)
                {
                    return "File is already uploaded.";
                }

                var directoryPath = Path.GetDirectoryName(uploadPath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                using (var stream = new FileStream(uploadPath, FileMode.Create))
                {
                    upload.file.CopyTo(stream);
                }

                EmployeeUploadedFile uploadedFile = new EmployeeUploadedFile
                {
                    EmployeeCode = employee.EmployeeCode,
                    EmployeeName = employee.EmployeeName,
                    FilePath = uploadPath,
                    CreatedBy = upload.createdBy,
                    CreatedUtc = DateTime.UtcNow,
                    IsDeleted = false
                };

                _context.EmployeeUploadedFiles.AddRange(uploadedFile);
                _context.SaveChanges();

                message = "File uploaded successfully.";

                return message;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred during file upload.", ex);
            }
        }

        public FileResult DownloadFile(string employeeCode, string fileName)
        {
            var uploadedFile = _context.EmployeeUploadedFiles
                .Where(file => file.EmployeeCode == employeeCode && file.IsDeleted == false)
                .AsEnumerable()
                .FirstOrDefault(file => Path.GetFileName(file.FilePath) == fileName);

            if (uploadedFile == null)
            {
                throw new FileNotFoundException("File not found.");
            }

            var filePath = uploadedFile.FilePath;

            if (!System.IO.File.Exists(filePath))
            {
                throw new FileNotFoundException("File not found on server.");
            }

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                stream.CopyTo(memory);
            }
            memory.Position = 0;

            var fileDownloadName = Path.GetFileName(filePath);

            return new FileStreamResult(memory, "application/octet-stream")
            {
                FileDownloadName = fileDownloadName
            };
        }

        public List<UploadedFileView> ViewUploadedFiles()
        {
            var uploadedFiles = _context.EmployeeUploadedFiles
                .Where(file => !file.IsDeleted)
                .ToList();

            var files = uploadedFiles.Select(file =>
                new UploadedFileView
                {
                    EmployeeCode = file.EmployeeCode,
                    FileName = Path.GetFileName(file.FilePath)
                }).ToList();

            return files;
        }

        public string EmployeeFileDelete(EmployeeFileDelete fileDelete, string employeeCode, string fileName)
        {
            var files = _context.EmployeeUploadedFiles
                .Where(f => f.EmployeeCode == employeeCode && f.IsDeleted == false)
                .AsEnumerable()
                .Where(f => Path.GetFileName(f.FilePath) == fileName)
                .ToList();

            if (!files.Any())
            {
                return "File not found.";
            }

            var file = files.First();
            file.UpdatedBy = fileDelete.UpdatedBy;
            file.UpdatedUtc = DateTime.UtcNow;
            file.IsDeleted = true;
            _context.SaveChanges();

            return "File deleted successfully.";
        }
    }
}
