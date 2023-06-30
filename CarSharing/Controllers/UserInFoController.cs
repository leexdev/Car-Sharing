using CarSharing.Models;
using CarSharing.Service;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CarSharing.Controllers
{
    public class UserInFoController : Controller
    {
        private readonly VehicleService vehicleService;
        private ApiService _apiService;

        public UserInFoController()
        {
            vehicleService = new VehicleService();
            _apiService = new ApiService();
        }

        [CustomAuthorize(Roles = "admin, user, partner")]
        public ActionResult TripList(int? status, Guid? vehicleTypeId)
        {
            var objTripModel = new VehicleManagementModel();
            var userId = (Guid)Session["Id"];
            List<Booking> listTrip = vehicleService.GetBookingsByUserId(userId).ToList();
            
            if (vehicleTypeId.HasValue)
            {
                listTrip = listTrip.Where(v => v.Vehicle.VehicleBrand.VehicleTypeId == vehicleTypeId).ToList();
            }

            if (status == 1 || status == null)
            {
                listTrip = listTrip.Where(v => v.Status == "Chờ duyệt" || v.Status == "Đã duyệt").ToList();
            }
            if (status == 2)
            {
                listTrip = listTrip.Where(v => v.Status == "Đã hoàn tất" || v.Status == "Đã hủy").ToList();
            }

            objTripModel.ListBookings = listTrip;
            objTripModel.ListVehicleTypes = vehicleService.GetVehicleTypes().ToList();
            return View(objTripModel);
        }

        [CustomAuthorize(Roles = "admin, user, partner")]
        public ActionResult TripListType(int? status, Guid? vehicleTypeId)
        {
            var objTripModel = new VehicleManagementModel();
            var userId = (Guid)Session["Id"];

            List<Booking> listTrip = vehicleService.GetBookingsByUserId(userId).ToList();

            if (vehicleTypeId.HasValue)
            {
                listTrip = listTrip.Where(v => v.Vehicle.VehicleBrand.VehicleTypeId == vehicleTypeId).ToList();
            }
            if (status == 1 || status == null)
            {
                listTrip = listTrip.Where(v => v.Status == "Chờ duyệt" || v.Status == "Đã duyệt").ToList();
            }
            if (status == 2)
            {
                listTrip = listTrip.Where(v => v.Status == "Đã hoàn tất" || v.Status == "Đã hủy").ToList();
            }
            objTripModel.ListBookings = listTrip;
            return PartialView("_ListBooking", objTripModel);
        }

        [CustomAuthorize(Roles = "admin, partner, user")]
        public ActionResult Information()
        {
            var objUserModel = new VehicleManagementModel();
            var user = vehicleService.GetUser((Guid)Session["Id"]);
            objUserModel.User = user;
            return View(objUserModel);
        }

        [CustomAuthorize(Roles = "admin, partner, user")]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [CustomAuthorize(Roles = "admin, partner, user")]
        public ActionResult TripDetail(Guid bookingId)
        {
            var objBookingModel = new VehicleManagementModel();
            var booking = vehicleService.GetBooking(bookingId);
            objBookingModel.Booking = booking;
            return View(objBookingModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = "admin, partner, user")]
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

        [CustomAuthorize(Roles = "admin, partner, user")]
        public ActionResult ListVehicles()
        {
            var objVehicleModel = vehicleService.GetHomeModel();
            return View(objVehicleModel);
        }


        [CustomAuthorize(Roles = "admin, partner")]
        public async Task<ActionResult> CreateVehicle()
        {
            var objVehicleMode = vehicleService.GetHomeModel();
            objVehicleMode.ListProvinces = await _apiService.GetProvincesAsync();
            return View(objVehicleMode);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [CustomAuthorize(Roles = "admin, partner")]
        public ActionResult CreateVehicle(Vehicle vehicle, District district, Province province, HttpPostedFileBase ImageUpLoad)
        {
            if (ModelState.IsValid)
            {
                var existingProvince = vehicleService.GetProvinceByCode(province.Code);
                var existingDistrict = vehicleService.GetDistrictByCode(district.code);

                if (existingProvince == null)
                {
                    vehicleService.AddProvince(province);
                }
                else
                {
                    province = existingProvince;
                }

                if (existingDistrict == null)
                {
                    district.province_code = province.Code;
                    vehicleService.AddDistrict(district);
                }
                else
                {
                    district = existingDistrict;
                }

                vehicle.VehicleId = Guid.NewGuid();
                vehicle.CodeDistrict = district.code;
                string fileName = Path.GetFileNameWithoutExtension(ImageUpLoad.FileName);
                string extension = Path.GetExtension(ImageUpLoad.FileName);
                fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                vehicle.ImageVehicle = "/Content/assets/img/" + fileName;
                ImageUpLoad.SaveAs(Path.Combine(Server.MapPath("~/Content/assets/img/"), fileName));
                vehicle.UserId = (Guid)Session["Id"];

                vehicleService.AddVehicle(vehicle);
                Session["SuccessMessage"] = "Thêm thành công!";
                return RedirectToAction("ListVehicles");
            }
            return RedirectToAction("Index");
        }


        [CustomAuthorize(Roles = "admin, partner")]
        public async Task<ActionResult> EditVehicle(Guid id)
        {
            var objVehicleMode = vehicleService.GetHomeModel();
            objVehicleMode.Vehicle = vehicleService.GetVehicle(id);
            objVehicleMode.ListProvinces = await _apiService.GetProvincesAsync();
            return View(objVehicleMode);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [CustomAuthorize(Roles = "admin, partner")]
        public ActionResult EditVehicle(Vehicle vehicle, Province province, District district, HttpPostedFileBase ImageUpLoad)
        {
            var existingProvince = vehicleService.GetProvinceByCode(province.Code);
            var existingDistrict = vehicleService.GetDistrictByCode(district.code);

            if (existingProvince == null)
            {
                vehicleService.AddProvince(province);
            }
            else
            {
                province = existingProvince;
            }

            if (existingDistrict == null)
            {
                district.province_code = province.Code;
                vehicleService.AddDistrict(district);
            }
            else
            {
                district = existingDistrict;
            }

            if (ImageUpLoad != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(ImageUpLoad.FileName);
                string extension = Path.GetExtension(ImageUpLoad.FileName);
                fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                vehicle.ImageVehicle = "/Content/assets/img/" + fileName;
                ImageUpLoad.SaveAs(Path.Combine(Server.MapPath("~/Content/assets/img/"), fileName));
            }
            else
            {
                vehicle.ImageVehicle = vehicle.ImageVehicle;
            }
            vehicleService.UpdateVehicle(vehicle, district);
            Session["SuccessMessage"] = "Cập nhật thành công!";
            return RedirectToAction("ListVehicles");
        }

        [HttpPost]
        [CustomAuthorize(Roles = "admin, partner")]
        public ActionResult Delete(Guid id)
        {
            var vehicle = vehicleService.GetVehicle(id);

            if (vehicle == null)
            {
                return HttpNotFound();
            }

            vehicleService.DeleteVehicle(vehicle);
            Session["SuccessMessage"] = "Xóa thành công!";
            return RedirectToAction("ListVehicles");
        }

        [CustomAuthorize(Roles = "admin, partner")]
        public ActionResult ListOrder(int? status, Guid? vehicleTypeId)
        {
            var objTripModel = new VehicleManagementModel();
            var userId = (Guid)Session["Id"];
            List<Booking> listOrder = vehicleService.GetOrders(userId).ToList();

            if (vehicleTypeId.HasValue)
            {
                listOrder = listOrder.Where(v => v.Vehicle.VehicleBrand.VehicleTypeId == vehicleTypeId).ToList();
            }
            if (status == 1 || status == null)
            {
                listOrder = listOrder.Where(v => v.Status == "Chờ duyệt").ToList();
            }
            if (status == 2)
            {
                listOrder = listOrder.Where(v => v.Status == "Đã duyệt").ToList();
            }
            if (status == 3)
            {
                listOrder = listOrder.Where(v => v.Status == "Đã hoàn tất" || v.Status == "Đã hủy").ToList();
            }

            objTripModel.ListBookings = listOrder;
            objTripModel.ListVehicleTypes = vehicleService.GetVehicleTypes().ToList();
            return View(objTripModel);
        }

        [CustomAuthorize(Roles = "admin, partner")]
        public ActionResult ListOrderType(int? status, Guid? vehicleTypeId)
        {
            var objOrderModel = new VehicleManagementModel();
            var userId = (Guid)Session["Id"];

            List<Booking> listOrder = vehicleService.GetOrders(userId).ToList();

            if (vehicleTypeId.HasValue)
            {
                listOrder = listOrder.Where(v => v.Vehicle.VehicleBrand.VehicleTypeId == vehicleTypeId).ToList();
            }
            if (status == 1 || status == null)
            {
                listOrder = listOrder.Where(v => v.Status == "Chờ duyệt").ToList();
            }
            if (status == 2)
            {
                listOrder = listOrder.Where(v => v.Status == "Đã duyệt").ToList();
            }
            if (status == 3)
            {
                listOrder = listOrder.Where(v => v.Status == "Đã hoàn tất" || v.Status == "Đã hủy").ToList();
            }
            objOrderModel.ListBookings = listOrder;
            return PartialView("_ListOrder", objOrderModel);
        }

        [CustomAuthorize(Roles = "admin, partner")]
        public ActionResult BrowserOrder(Guid bookingId)
        {
            var booking = vehicleService.GetBooking(bookingId);
            booking.Status = "Đã duyệt";
            vehicleService.UpdateBooking(booking);
            return RedirectToAction("ListOrder");
        }

        [CustomAuthorize(Roles = "admin, partner")]
        public ActionResult SuccessOrder(Guid bookingId)
        {
            var booking = vehicleService.GetBooking(bookingId);
            booking.Status = "Đã hoàn tất";
            vehicleService.UpdateBooking(booking);
            return RedirectToAction("ListOrder");
        }

        [CustomAuthorize(Roles = "admin, partner")]
        public ActionResult CancelOrder(Guid bookingId)
        {
            var booking = vehicleService.GetBooking(bookingId);
            booking.Status = "Đã hủy";
            vehicleService.UpdateBooking(booking);
            return RedirectToAction("ListOrder");
        }

        [CustomAuthorize(Roles = "admin, partner, user")]
        public ActionResult CancelOrderBooking(Guid bookingId)
        {
            var booking = vehicleService.GetBooking(bookingId);
            booking.Status = "Đã hủy";
            vehicleService.UpdateBooking(booking);
            return RedirectToAction("ListOrder");
        }

        [CustomAuthorize(Roles = "admin, partner")]
        public JsonResult GetVariants(Guid vehicleTypeId)
        {
            var variants = vehicleService.GetVariantsByVehicleType(vehicleTypeId);

            var variantsData = variants.Select(v => new
            {
                VariantId = v.VariantId,
                VariantName = v.VariantName
            });

            return Json(variantsData, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(Roles = "admin, partner")]
        public JsonResult GetBrands(Guid vehicleTypeId)
        {
            var brands = vehicleService.GetBrandsByVehicleType(vehicleTypeId);

            var brandsData = brands.Select(b => new
            {
                BrandId = b.BrandId,
                BrandName = b.BrandName
            });

            return Json(brandsData, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(Roles = "admin, partner")]
        public async Task<JsonResult> GetDistrics(int provinceCode)
        {
            var districts = await _apiService.GetDistrictsByProvinceAsync(provinceCode);

            var districtsData = districts.Select(d => new
            {
                code = d.code,
                name = d.name,
                province_code = d.province_code
            });

            return Json(districtsData, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase upload)
        {
            if (upload != null && upload.ContentLength > 0)
            {
                var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Guid.NewGuid().ToString("N") + Path.GetExtension(upload.FileName);
                var imagePath = Path.Combine(Server.MapPath("~/Content/assets/img/"), fileName);
                upload.SaveAs(imagePath);

                var imageUrl = Url.Content("~/Content/assets/img/" + fileName);
                return Content("<script>window.parent.CKEDITOR.tools.callFunction(" + Request.QueryString["CKEditorFuncNum"] + ", '" + imageUrl + "');</script>");
            }

            return HttpNotFound();
        }
    }
}