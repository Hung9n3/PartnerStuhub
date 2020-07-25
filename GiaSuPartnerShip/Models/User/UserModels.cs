using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace GiaSuPartnerShip.Models.User
{
    public class UserModels : IdentityUser
    {
        public ICollection<LocationInfo> MyLocationInfo { get; set; }
    }
}
