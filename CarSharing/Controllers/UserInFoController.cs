using CarSharing.Models;
using CarSharing.Service;
using System;
using System.Collections.Generic;
using System.IO;
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
            var objUserModel = new VehicleManagementModel();
            var user = vehicleService.GetUser((Guid)Session["Id"]);
            objUserModel.User = user;
            return View(objUserModel);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }


        public ActionResult TripDetail()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateInformation(User user, HttpPostedFileBase ImageUpLoad)
        {
            if (ImageUpLoad != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(ImageUpLoad.FileName);
                string extension = Path.GetExtension(ImageUpLoad.FileName);
                fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                user.Avatar = "/Content/assets/img/" + fileName;
                ImageUpLoad.SaveAs(Path.Combine(Server.MapPath("~/Content/assets/img/"), fileName));
                vehicleService.UpdateAvatar(user);
                TempData["Message"] = "Thay đổi ảnh thành công!";
                return RedirectToAction("Information");
            }

            vehicleService.UpdateUser(user);
            TempData["Message"] = "Cập nhật thông tin thành công!";
            return RedirectToAction("Information");
        }
    }
}