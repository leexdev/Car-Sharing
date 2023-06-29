using CarSharing.Helpers;
using CarSharing.Models;
using CarSharing.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarSharing.Controllers
{
    public class VehicleController : Controller
    {
        private VehicleService vehicleService;

        public VehicleController()
        {
            vehicleService = new VehicleService();
        }
        public ActionResult Detail(Guid id)
        {
            Session["returnUrl"] = Request.Url.ToString();
            VehicleManagementModel objDetailModel = vehicleService.GetDetailModel();
            objDetailModel.Vehicle = vehicleService.GetVehicles().Where(n => n.VehicleId == id).FirstOrDefault();

            objDetailModel.ListReviews = vehicleService.GetReviews().Where(n => n.VehicleId == id).ToList();
            return View(objDetailModel);
        }
    }
}