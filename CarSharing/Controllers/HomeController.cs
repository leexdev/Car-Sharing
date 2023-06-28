using CarSharing.Helpers;
using CarSharing.Models;
using CarSharing.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CarSharing.Controllers
{
    public class HomeController : Controller
    {
        private VehicleService vehicleService;
        private ApiService _apiService;

        public HomeController()
        {
            vehicleService = new VehicleService();
        }
        public async Task<ActionResult> Index()
        {
            VehicleManagementModel objVehicleModel = vehicleService.GetHomeModel();
            return View(objVehicleModel);
        }

        public JsonResult GetProvinces(Guid vehicleTypeId)
        {
            var provinces = vehicleService.GetProvinceByVehicleType(vehicleTypeId);

            var provincesData = provinces.Select(b => new
            {
                Code = b.Code,
                Name = b.Name
            });

            return Json(provincesData, JsonRequestBehavior.AllowGet);
        }
    }
}