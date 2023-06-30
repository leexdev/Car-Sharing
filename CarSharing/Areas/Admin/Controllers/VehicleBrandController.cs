using CarSharing.Models;
using CarSharing.Service;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CarSharing.Areas.Admin.Controllers
{
    public class VehicleBrandController : Controller
    {
        private VehicleService vehicleService;

        public VehicleBrandController()
        {
            vehicleService = new VehicleService();
        }

        [CustomAuthorize(Roles = "admin")]
        public async Task<ActionResult> Index(string currentFilter, string searchString, int? page)
        {
            var objVehicleBrandModel = vehicleService.GetBrandModel();

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
                objVehicleBrandModel.ListBrands = vehicleService.GetVehicleBrandsSearch(searchString).ToList();
            }

            ViewBag.CurrentFilter = searchString;
            int pageSize = 5;
            int pageNumber = page ?? 1;
            var pagedVehicleBrands = objVehicleBrandModel.ListBrands.ToPagedList(pageNumber, pageSize);
            var pagedVehicleBrandModels = new StaticPagedList<VehicleManagementModel>(
                pagedVehicleBrands.Select(a => new VehicleManagementModel
                {
                    ListBrands = new List<VehicleBrand> { a }
                }),
                pagedVehicleBrands.GetMetaData()
            );
            objVehicleBrandModel.PagedVehicleBrands = pagedVehicleBrandModels;
            return View(objVehicleBrandModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult Create(VehicleBrand vehicleBrand)
        {

            vehicleBrand.BrandId = Guid.NewGuid();

            vehicleService.AddVehicleBrand(vehicleBrand);
            Session["SuccessMessage"] = "Thêm thành công!";
            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult Edit(VehicleBrand vehicleBrand)
        {
            vehicleService.UpdateVehicleBrand(vehicleBrand);
            Session["SuccessMessage"] = "Cập nhật thành công!";
            return Redirect(Request.UrlReferrer.ToString());
        }


        [HttpPost]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult Delete(Guid id)
        {
            var vehicleBrand = vehicleService.GetVehicleBrand(id);

            if (vehicleBrand == null)
            {
                return HttpNotFound();
            }

            vehicleService.DeleteVehicleBrand(vehicleBrand);
            Session["SuccessMessage"] = "Xóa thành công!";
            return RedirectToAction("Index");
        }
    }
}