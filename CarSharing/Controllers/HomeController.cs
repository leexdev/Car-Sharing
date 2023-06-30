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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User user)
        {
            var check = vehicleService.CheckEmailExists(user.Email);
            if (check != null)
            {
                TempData["ErrorMessage"] = "Email đã tồn tại!";
                return Json(new { success = false, message = TempData["ErrorMessage"] });
            }
            if (user.Password != user.ConfirmPassword)
            {
                TempData["ErrorMessage"] = "Mật khẩu và mật khẩu xác nhận không khớp!";
                return Json(new { success = false, message = TempData["ErrorMessage"] });
            }
            user.UserId = Guid.NewGuid();
            user.Avatar = "/Content/assets/img/user.png";
            user.Role = "user";
            user.Password = vehicleService.GetMD5(user.Password);
            user.isDeleted = false;

            vehicleService.AddUser(user);
            TempData["SuccessMessage"] = "Đăng ký thành công!";
            return Json(new { success = true });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user)
        {
            ModelState.Remove("User.ConfirmPassword");
            var data = vehicleService.CheckEmailExists(user.Email);
            if (data == null || data.Password != vehicleService.GetMD5(user.Password))
            {
                TempData["ErrorMessage"] = "Tên đăng nhập hoặc mật khẩu không chính xác!";
                return Json(new { success = false, message = TempData["ErrorMessage"] });
            }
            else
            {
                Session["Id"] = data.UserId;
                Session["Name"] = data.FullName;
                Session["Avatar"] = data.Avatar;
                Session["Email"] = data.Email;
                Session["Phone"] = data.Phone;
                Session["Address"] = data.Address;
                Session["Role"] = data.Role;
                return Json(new { success = true });
            }
        }


        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }
    }
}