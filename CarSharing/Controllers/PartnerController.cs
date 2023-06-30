using CarSharing.Models;
using CarSharing.Service;
using System;
using System.Web.Mvc;

namespace CarSharing.Controllers
{
    public class PartnerController : Controller
    {
        private readonly VehicleService vehicleService;

        public PartnerController()
        {
            vehicleService = new VehicleService();
        }

        [CustomAuthorize(Roles = "user")]
        public ActionResult Index()
        {
            var userId = new Guid(Session["Id"].ToString());
            var user = vehicleService.GetUser(userId);

            var objUserModel = new VehicleManagementModel();
            objUserModel.User = user;
            return View(objUserModel);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "user")]
        public ActionResult SendPartner(User user)
        {
            user.UserId = (Guid)Session["Id"];
            user.PartnerRequest = true;
            vehicleService.UpdateUser(user);
            return View("Success");
        }

        public ActionResult Success()
        {
            return View();
        }
    }
}