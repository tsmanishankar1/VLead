using Microsoft.Extensions.Configuration;
using Vis.VleadProcessV3.Repositories;
using Vis.VleadProcessV3.ViewModels;

namespace Vis.VleadProcessV3.Services
{
    public class BJWishService
    {
        private readonly UnitViewWork _unitviewWork;
        public BJWishService(UnitViewWork unitViewWork)
        {
            _unitviewWork = unitViewWork;
        }
        public Object GetBJWishData()
        {
            DateTime systemdate = DateTime.Now;
            DateTime getcurrentdt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(systemdate, TimeZoneInfo.Local.Id, "India Standard Time");
            DateTime Scurrentdate = getcurrentdt.Date;
            string Date = Scurrentdate.ToString("yyyy") + "-" + Scurrentdate.ToString("MM") + "-" + Scurrentdate.ToString("dd");
            string MD = Scurrentdate.ToString("MM") + "-" + Scurrentdate.ToString("dd");
            string Y = Scurrentdate.ToString("yyyy");
            string M = Scurrentdate.ToString("MM");
            string D = Scurrentdate.ToString("dd");
            int Anniversary = 0;
            var DOBObjList = new List<DOBWishViewModel>();
            var DOJObjList = new List<DOJWishViewModel>();
            var result = new Object();

            var checkDOBDOJ = _unitviewWork.ViewGetBirthandJoining.Where(x => x.Dobmd == MD || x.Dojmd == MD).ToList();
            foreach (var item in checkDOBDOJ)
            {
                //DOB
                if (item.Dobmd == MD)
                {
                    DOBObjList.Add(new DOBWishViewModel
                    {
                        EmployeeCode = item.EmployeeCode,
                        EmployeeName = item.EmployeeName,
                        Department = item.Department,
                        Designation = item.Designation,
                        DateOfBirth = Convert.ToDateTime(item.Dobdate),
                        Code = "BirthDay"
                    });
                    //add birthday count in collection
                }
                //DOJ
                if (item.Dojmd == MD)
                {
                    //add joining count in collection
                    Anniversary = Convert.ToInt16(Y) - Convert.ToInt16(item.Dojy);
                    DOJObjList.Add(new DOJWishViewModel
                    {
                        EmployeeCode = item.EmployeeCode,
                        EmployeeName = item.EmployeeName,
                        Department = item.Department,
                        Designation = item.Designation,
                        Anniversary = Anniversary,
                        DateOfJoining = Convert.ToDateTime(item.Dojdate),
                        Code = "Joining"
                    });
                }
            }
            result = new
            {
                DOBObjList = DOBObjList,
                DOJObjList = DOJObjList,
            };
            return result;
        }
    }
}
