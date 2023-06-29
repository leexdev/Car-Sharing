using CarSharing.Helpers;
using CarSharing.Models;
using CarSharing.Service;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;

namespace CarSharing.Controllers
{
    public class SearchController : Controller
    {
        private readonly VehicleService vehicleService;

        public SearchController()
        {
            vehicleService = new VehicleService();
        }

        //public ActionResult Index(string currentFilter, int? page, Guid vehicleTypeId, int provinceCode)
        //{
        //    var objVehicleModel = vehicleService.GetHomeModel();
        //    int pagesize = 5;
        //    int pagenumber = page ?? 1;
        //    objVehicleModel.ListVehicles = vehicleService.SearchVehicles(vehicleTypeId, provinceCode);
        //    var pagedVehicles = objVehicleModel.ListVehicles.ToPagedList(pagenumber, pagesize);
        //    var pagedVehiclModels = new StaticPagedList<VehicleManagementModel>(
        //        pagedVehicles.Select(a => new VehicleManagementModel
        //        {
        //            ListVehicles = new List<Vehicle> { a }
        //        }),
        //        pagedVehicles.GetMetaData()
        //    );
        //    objVehicleModel.PagedVehicles = pagedVehiclModels;
        //    return View(objVehicleModel);
        //}

        public ActionResult Index(string currentFilter, int? page, Guid? vehicleTypeId, Guid? variantId, Guid? brandId, int? provinceCode, int? districtCode)
        {
            ViewBag.CurrentFilter = currentFilter;
            ViewBag.vehicleTypeId = vehicleTypeId;
            ViewBag.variantId = variantId;
            ViewBag.brandId = brandId;
            ViewBag.provinceCode = provinceCode;
            ViewBag.districtCode = districtCode;
            var objVehicleModel = vehicleService.GetHomeModel();
            List<Vehicle> listVehicle = vehicleService.GetVehicles().ToList();

            if (vehicleTypeId.HasValue)
            {
                listVehicle = listVehicle.Where(x => x.VehicleBrand.VehicleTypeId == vehicleTypeId).ToList();
            }

            if (variantId.HasValue)
            {
                listVehicle = listVehicle.Where(x => x.VariantId == variantId).ToList();
            }

            if (brandId.HasValue)
            {
                listVehicle = listVehicle.Where(x => x.BrandId == brandId).ToList();
            }

            if (provinceCode.HasValue)
            {
                listVehicle = listVehicle.Where(x => x.District.province_code == provinceCode).ToList();
            }

            if (districtCode.HasValue)
            {
                listVehicle = listVehicle.Where(x => x.CodeDistrict == districtCode).ToList();
            }

            objVehicleModel.ListVehicles = listVehicle;
            int pagesize = 5;
            int pagenumber = page ?? 1;
            var pagedVehicles = objVehicleModel.ListVehicles.ToPagedList(pagenumber, pagesize);
            var pagedVehiclModels = new StaticPagedList<VehicleManagementModel>(
                pagedVehicles.Select(a => new VehicleManagementModel
                {
                    ListVehicles = new List<Vehicle> { a }
                }),
                pagedVehicles.GetMetaData()
            );
            objVehicleModel.PagedVehicles = pagedVehiclModels;
            //return PartialView("_ListVehicleSearch", objVehicleModel);
            return View(objVehicleModel);
        }

        public ActionResult SearchVehicle(string currentFilter, int? page, Guid? vehicleTypeId, Guid? variantId, Guid? brandId, int? provinceCode, int? districtCode)
        {
            Session["returnUrl"] = Request.Url.ToString();
            ViewBag.CurrentFilter = currentFilter;
            ViewBag.vehicleTypeId = vehicleTypeId;
            ViewBag.variantId = variantId;
            ViewBag.brandId = brandId;
            ViewBag.provinceCode = provinceCode;
            ViewBag.districtCode = districtCode;
            var objVehicleModel = vehicleService.GetHomeModel();
            List<Vehicle> listVehicle = vehicleService.GetVehicles().ToList();

            if (vehicleTypeId.HasValue)
            {
                listVehicle = listVehicle.Where(x => x.VehicleBrand.VehicleTypeId == vehicleTypeId).ToList();
                if(objVehicleModel.ListVehicleVariants.Where(n => n.VehicleTypeId == vehicleTypeId && n.VariantId == variantId).ToList().Count == 0)
                {
                    variantId = null;
                }
                if (objVehicleModel.ListBrands.Where(n => n.VehicleTypeId == vehicleTypeId && n.BrandId == brandId).ToList().Count == 0)
                {
                    brandId = null;
                }
            }

            if (variantId.HasValue)
            {
                listVehicle = listVehicle.Where(x => x.VariantId == variantId).ToList();
            }

            if (brandId.HasValue)
            {
                listVehicle = listVehicle.Where(x => x.BrandId == brandId).ToList();
            }

            if (provinceCode.HasValue)
            {
                listVehicle = listVehicle.Where(x => x.District.province_code == provinceCode).ToList();
            }

            if (districtCode.HasValue)
            {
                listVehicle = listVehicle.Where(x => x.CodeDistrict == districtCode).ToList();
            }

            objVehicleModel.ListVehicles = listVehicle;

            int pagesize = 5;
            int pagenumber = page ?? 1;
            var pagedVehicles = objVehicleModel.ListVehicles.ToPagedList(pagenumber, pagesize);
            var pagedVehiclModels = new StaticPagedList<VehicleManagementModel>(
                pagedVehicles.Select(a => new VehicleManagementModel
                {
                    ListVehicles = new List<Vehicle> { a }
                }),
                pagedVehicles.GetMetaData()
            );
            objVehicleModel.PagedVehicles = pagedVehiclModels;
            return PartialView("_ListVehicleSearch", objVehicleModel);
        }

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

        public async Task<JsonResult> GetDistrics(int provinceCode)
        {
            var districts = vehicleService.GetDistrictsByProvince(provinceCode);

            var districtsData = districts.Select(d => new
            {
                code = d.code,
                name = d.name,
                province_code = d.province_code
            });

            return Json(districtsData, JsonRequestBehavior.AllowGet);
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