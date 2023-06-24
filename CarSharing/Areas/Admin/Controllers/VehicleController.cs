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
            else
            {
                objVehicleModel.ListVehicles = vehicleService.GetVehicles().ToList();
            }

            ViewBag.CurrentFilter = searchString;
            int pageSize = 5;
            int pageNumber = page ?? 1;
            var pagedArticles = objVehicleModel.ListVehicles.ToPagedList(pageNumber, pageSize);
            var pagedArticleModels = new StaticPagedList<VehicleManagementModel>(
                pagedArticles.Select(a => new VehicleManagementModel
                {
                    ListVehicles = new List<Vehicle> { a }
                }),
                pagedArticles.GetMetaData()
            );
            objVehicleModel.PagedVehicles = pagedArticleModels;
            objVehicleModel.ListProvinces = await _apiService.GetProvincesAsync();
            return View(objVehicleModel);
        }

        public JsonResult GetVariants(Guid vehicleTypeId)
        {
            // Lấy danh sách variants dựa trên vehicleTypeId
            var variants = vehicleService.GetVariantsByVehicleType(vehicleTypeId);

            // Chỉ lấy các trường cần thiết để truyền về cho dropdownlist
            var variantsData = variants.Select(v => new
            {
                VariantId = v.VariantId,
                VariantName = v.VariantName
            });

            return Json(variantsData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBrands(Guid vehicleTypeId)
        {
            // Lấy danh sách variants dựa trên vehicleTypeId
            var brands = vehicleService.GetBrandsByVehicleType(vehicleTypeId);

            // Chỉ lấy các trường cần thiết để truyền về cho dropdownlist
            var brandsData = brands.Select(b => new
            {
                BrandId = b.BrandId,
                BrandName = b.BrandName
            });

            return Json(brandsData, JsonRequestBehavior.AllowGet);
        }

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


        [HttpGet]
        public ActionResult Create(Guid id)
        {
            var data = vehicleService.GetVehicle(id);

            var model = new VehicleManagementModel
            {
                Vehicle = data
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Vehicle vehicle, District district, Province province, HttpPostedFileBase ImageUpLoad)
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
            //if (Session["Id"] is Guid authorId)
            string userIdString = "F5FC5234-9824-48B4-87D3-1861DF176627";
            vehicle.UserId = Guid.Parse(userIdString);

            vehicleService.AddVehicle(vehicle);
            Session["SuccessMessage"] = "Thêm thành công!";
            return Redirect(Request.UrlReferrer.ToString());
        }

    }
}