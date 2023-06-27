using CarSharing.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarSharing.Controllers
{
    public class UserInFoController : Controller
    {
        private readonly VehicleService vehicleService;

        public UserInFoController()
        {
            vehicleService = new VehicleService();
        }
        public ActionResult TripList()
        {
            return View();
        }

        public ActionResult Information()
        {
            return View();
        }

        public ActionResult ChangePassword()
        {
            return View();
        }


        public ActionResult TripDetail()
        {
            return View();
        }
    }
}