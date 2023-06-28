using CarSharing.Models;
using CarSharing.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarSharing.Controllers
{
    public class PayController : Controller
    {
        private readonly VehicleService vehicleService;

        public PayController()
        {
            vehicleService = new VehicleService();
        }

        public ActionResult Index(Guid id)
        {
            var vehicle = vehicleService.GetVehicle(id);
            VehicleManagementModel objVehicleModel = new VehicleManagementModel();
            objVehicleModel.Vehicle = vehicle;
            return View(objVehicleModel);
        }
    }
}