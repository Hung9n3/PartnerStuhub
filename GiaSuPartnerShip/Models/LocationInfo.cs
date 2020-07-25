using GiaSuPartnerShip.Models.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GiaSuPartnerShip.Models
{
    public class LocationInfo
    {
        [Key]
        public int LocationID { get; set; }
        [Required]
        public string Name { get; set; }
        //public string LogoUrl { get; set; }
        //public bool AirConditioned { get; set; }
        public string Menu { get; set; }
        //public ICollection<string> ImageUrl { get; set; }
        public District District { get; set; }
        public bool Type { get; set; }
        public UserModels Owner { get; set; }
    }
}
