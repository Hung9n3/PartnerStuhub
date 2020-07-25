using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiaSuPartnerShip.Models
{
    public class District
    {
        public int DistrictID { get; set; }
        public string DistrictName { get; set; }
        public City city { get; set; }
    }
}
