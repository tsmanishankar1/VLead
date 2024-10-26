using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vis.VleadProcessV3.Models;

namespace Vis.VleadProcessV3.Services
{
    public class HolidayMasterService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        //private readonly string uploadFile = "HolidayMaster\\UploadedFiles";

        public HolidayMasterService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;

            //var filePath = Path.Combine(Directory.GetCurrentDirectory(), uploadFile);
            //if (!Directory.Exists(filePath))
            //{
            //    Directory.CreateDirectory(filePath);
            //}
        }

        //public string UploadHolidayExcelFile(PassExcel excel)
        //{
        //    try
        //    {
        //        string uniqueFileName = excel.file.FileName;
        //        var filePath = Path.Combine(Directory.GetCurrentDirectory(), uploadFile, uniqueFileName);

        //        int count = 1;
        //        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(uniqueFileName);
        //        string fileExtension = Path.GetExtension(uniqueFileName);

        //        while (File.Exists(filePath))
        //        {
        //            uniqueFileName = $"{fileNameWithoutExtension}({count}){fileExtension}";

        //            filePath = Path.Combine(Directory.GetCurrentDirectory(), uploadFile, uniqueFileName);

        //            count++;
        //        }

        //        using (var fileStream = new FileStream(filePath, FileMode.Create))
        //        {
        //            excel.file.CopyTo(fileStream);
        //        }

        //        string message = "";
        //        if (excel.file != null)
        //        {
        //            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        //            using (var stream = excel.file.OpenReadStream())
        //            {
        //                using (var reader = ExcelReaderFactory.CreateReader(stream))
        //                {
        //                    var result = reader.AsDataSet();
        //                    var data = new List<HolidayMaster>();
        //                    if (result != null && result.Tables.Count > 0)
        //                    {
        //                        System.Data.DataTable dtStudentRecords = result.Tables[0];
        //                        if (dtStudentRecords.Rows.Count > 1)
        //                        {
        //                            foreach (DataRow row in dtStudentRecords.Rows)
        //                            {
        //                                if (row.Table.Rows.IndexOf(row) == 0)
        //                                    continue;
        //                                string uniqueIdentifier = Convert.ToString(row[0]);
        //                                string holidayType = Convert.ToString(row[2]);
        //                                HolidayMaster existingRecord = _applicationDbContext.HolidayMasters.FirstOrDefault(h => h.HolidayDescription == uniqueIdentifier && h.HolidayType == holidayType);

        //                                if (existingRecord != null)
        //                                {
        //                                    continue;
        //                                }

        //                                string dateFormat = "dd-MM-yyyy HH:mm:ss";
        //                                HolidayMaster holiday = new HolidayMaster
        //                                {
        //                                    HolidayDescription = uniqueIdentifier,
        //                                    HolidayDate = DateTime.ParseExact(row[1].ToString(), dateFormat, CultureInfo.InvariantCulture),
        //                                    HolidayType = holidayType,
        //                                    IsDelete = false,
        //                                    CreatedBy = excel.createdBy,
        //                                    CreatedByUtc = DateTime.UtcNow
        //                                };

        //                                data.Add(holiday);
        //                            }

        //                            _applicationDbContext.AddRange(data);

        //                            int output = _applicationDbContext.SaveChanges();
        //                            if (output > 0)
        //                                message = "The Excel file has been successfully uploaded.";
        //                            else
        //                                throw new CustomException("No new records to upload.");
        //                        }
        //                        else
        //                            throw new MessageNotFoundException("Selected file is empty");
        //                    }
        //                    else
        //                        throw new MessageNotFoundException("Selected file is empty");
        //                }
        //            }
        //        }
        //        else
        //        {
        //            throw new Exception("Invalid File.");
        //        }
        //        return message;
        //    }
        //    catch (MessageNotFoundException ex)
        //    {
        //        throw new MessageNotFoundException("Selected file is empty");
        //    }
        //    catch (CustomException ex)
        //    {
        //        throw new CustomException("The Excel file is already uploaded.");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Invalid File.");
        //    }
        //}

        public HolidayYear GetHolidayYears()
        {
            try
            {
                List<int> years = _applicationDbContext.HolidayMasters
                    .Select(h => h.HolidayDate.Year)
                    .Distinct()
                    .ToList();

                var response = new HolidayYear
                {
                    year = years
                };

                return response;
            }
            catch (MessageNotFoundException ex)
            {
                throw new MessageNotFoundException($"No years found.");
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to retrieve years.");
            }
        }
        public List<HolidayMaster> GetHolidayListByYear(int year)
        {
            try
            {
                var holidays = _applicationDbContext.HolidayMasters
                    .Where(h => h.HolidayDate.Year == year)
                    .ToList();

                return holidays;
            }
            catch (MessageNotFoundException ex)
            {
                throw new MessageNotFoundException($"No holidays found for the year {year}");
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to retrieve holidays.");
            }
        }
    }
}