using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.ViewModels
{
    public class LocationViewModel
    {
        public string Locality { get; set; }
        public int LocalityId { get; set; }
        public string City { get; set; }
        public int CityId { get; set; }
        public string State { get; set; }
        public int StateId { get; set; }
        public string Country { get; set; }
        public int CountryId { get; set; }
    }
}
