using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using GiaSuPartnerShip.Models;
using GiaSuPartnerShip.Models.Modifilter;
using GiaSuPartnerShip.Models.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiaSuPartnerShip.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LocationInfoController : ControllerBase
    {
        private AppDbContext _db;
        private UserManager<UserModels> _userManager;
        public LocationInfoController(AppDbContext _db, UserManager<UserModels> _userManager)
        {
            this._db = _db;
            this._userManager = _userManager;
        }
        LocationInfo Result = new LocationInfo();
        [HttpPost]
        public District AddDistrict([FromBody] DistrictModel districtModel)
        {
            var city = _db.Cities.FirstOrDefault(x => x.CityID == districtModel.CityID);
            var district = new District();
            district.city = city;
            district.DistrictName = districtModel.DistrictName;
            _db.Districts.Add(district);
            _db.SaveChanges();
            return district;
        }
        //[HttpPost]
        //public LocationInfo AddLocationInfo([FromBody] LocationModel locationModel)
        //{
        //    var district = _db.Districts.FirstOrDefault(x => x.DistrictID == locationModel.DistrictID);
        //    var location = new LocationInfo();
        //    location.District = district;
        //    location.Name = locationModel.Name;
        //    location.Type = locationModel.Type;
        //    location.Menu = locationModel.Menu;
        //    _db.LocationInfos.Add(location);
        //    _db.SaveChanges();
        //    return location;
        //}
        //[HttpGet]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //public List<GetLocationModel> GetLocationInfos()
        //{

        //    var location = _db.LocationInfos.Include(x => x.District)
        //                                    .Select(x => new GetLocationModel
        //                                    {
        //                                        Name = x.Name,
        //                                        DistrictName = x.District.DistrictName,
        //                                        CityName = x.District.city.CityName,
        //                                        Menu = x.Menu,
        //                                        Type = x.Type,
        //                                        username = x.Owner.UserName
        //                                    }
        //                                        ).ToList();
        //    return location;
        //}
        //[HttpGet("{id}")]
        //public GetLocationModel GetLocationInfoByID(int id)
        //{
        //    var location = _db.LocationInfos.Include(x => x.District)
        //        .Select(x => new GetLocationModel
        //        {
        //            Name = x.Name,
        //            DistrictName = x.District.DistrictName,
        //            CityName = x.District.city.CityName,
        //            Menu = x.Menu,
        //            Type = x.Type
        //        }).FirstOrDefault();
        //    return location;
        //}
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> AddLocate(LocationModel locationModel)
        {
            string userid = User.Claims.First(c => c.Type == "UserId").Value;
            var user = await _userManager.FindByIdAsync(userid);
            var district = _db.Districts.FirstOrDefault(x => x.DistrictID == locationModel.DistrictID);
            var location = new LocationInfo();
            location.District = district;
            location.Name = locationModel.Name;
            location.Type = locationModel.Type;
            location.Menu = locationModel.Menu;
            location.Owner = user;
            _db.LocationInfos.Add(location);
            _db.SaveChanges();
            return Ok(user);
        }
        

        [HttpGet("{Name}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult>  GetLocationForUpdate(string Name)
        {
            
            string userid = User.Claims.First(c => c.Type == "UserId").Value;
            var user = await _userManager.FindByIdAsync(userid);
            var location = _db.LocationInfos.Where(x => x.Owner.UserName == user.UserName).ToList();
            foreach (LocationInfo Locate in location)
            {
                if(Locate.Name == Name)
                {
                    Result = Locate;
                    break;
                }
            }
            GetLocationModel locationModel = new GetLocationModel
            {
                CityName = Result.District.city.CityName,
                DistrictName = Result.District.DistrictName,
                Menu = Result.Menu,
                Type = Result.Type,
                Name = Result.Name
            };
            return Ok(locationModel);
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public void UpdateLocationInfo(LocationInfo location)
        {
            _db.LocationInfos.Update(location);
        }
        [HttpGet("{Name}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public List<string> SearchToUpdate(string Name)
        {
            List<string> name = new List<string>();
            string userid = User.Claims.First(c => c.Type == "UserId").Value;
            var Location = _db.LocationInfos.Where(x => x.Owner.Id == userid && x.Name.Contains(Name)).ToList();           
            foreach (LocationInfo location in Location)
            {
                 name.Add(location.Name);
            }
            return name;
        }
    }
}
