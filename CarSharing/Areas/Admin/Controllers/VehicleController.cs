using CarSharing.Models;
using CarSharing.Service;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CarSharing.Areas.Admin.Controllers
{
    public class VehicleController : Controller
    {
        private VehicleService vehicleService;
        private ApiService _apiService;

        public VehicleController()
        {
            vehicleService = new VehicleService();
            _apiService = new ApiService();
        }

        [CustomAuthorize(Roles = "admin")]
        public async Task<ActionResult> Index(string currentFilter, string searchString, int? page)
        {
            var objVehicleModel = vehicleService.GetHomeModel();

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                objVehicleModel.ListVehicles = vehicleService.GetVehiclesSearch(searchString).ToList();
            }

            ViewBag.CurrentFilter = searchString;
            int pageSize = 5;
            int pageNumber = page ?? 1;
            var pagedVehicles = objVehicleModel.ListVehicles.ToPagedList(pageNumber, pageSize);
            var pagedVehiclModels = new StaticPagedList<VehicleManagementModel>(
                pagedVehicles.Select(a => new VehicleManagementModel
                {
                    ListVehicles = new List<Vehicle> { a }
                }),
                pagedVehicles.GetMetaData()
            );
            objVehicleModel.PagedVehicles = pagedVehiclModels;
            objVehicleModel.ListProvinces = await _apiService.GetProvincesAsync();
            return View(objVehicleModel);
        }


        [CustomAuthorize(Roles = "admin")]
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


        [CustomAuthorize(Roles = "admin")]
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

        [CustomAuthorize(Roles = "admin")]
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
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult Create(Vehicle vehicle, District district, Province province, HttpPostedFileBase ImageUpLoad)
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
                return Redirect(Request.UrlReferrer.ToString());
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult Edit(Vehicle vehicle, Province province, District district, HttpPostedFileBase ImageUpLoad)
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
            return Redirect(Request.UrlReferrer.ToString());
        }


        [HttpPost]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult Delete(Guid id)
        {
            var vehicle = vehicleService.GetVehicle(id);

            if (vehicle == null)
            {
                return HttpNotFound();
            }

            vehicleService.DeleteVehicle(vehicle);
            Session["SuccessMessage"] = "Xóa thành công!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [CustomAuthorize(Roles = "admin")]
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