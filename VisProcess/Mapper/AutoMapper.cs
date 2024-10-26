using AutoMapper;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.ViewModels;

namespace VisProcess.Mapper
{
    public class AutoMapper:Profile
    {
        public AutoMapper()
        {
            CreateMap<InwardExcel, JobOrder>().ReverseMap();
        }
    }
}
