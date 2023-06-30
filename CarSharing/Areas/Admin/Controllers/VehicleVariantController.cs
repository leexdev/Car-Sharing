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
    public class VehicleVariantController : Controller
    {
        private VehicleService vehicleService;

        public VehicleVariantController()
        {
            vehicleService = new VehicleService();
        }

        [CustomAuthorize(Roles = "admin")]
        public async Task<ActionResult> Index(string currentFilter, string searchString, int? page)
        {
            var objVehicleVariantModel = vehicleService.GetVariantModel();

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
                objVehicleVariantModel.ListVehicleVariants = vehicleService.GetVehicleVariantsSearch(searchString).ToList();
            }

            ViewBag.CurrentFilter = searchString;
            int pageSize = 5;
            int pageNumber = page ?? 1;
            var pagedVehicleVariants = objVehicleVariantModel.ListVehicleVariants.ToPagedList(pageNumber, pageSize);
            var pagedVehicleVariantModels = new StaticPagedList<VehicleManagementModel>(
                pagedVehicleVariants.Select(a => new VehicleManagementModel
                {
                    ListVehicleVariants = new List<VehicleVariant> { a }
                }),
                pagedVehicleVariants.GetMetaData()
            );
            objVehicleVariantModel.PagedVehicleVariants = pagedVehicleVariantModels;
            return View(objVehicleVariantModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult Create(VehicleVariant vehicleVariant)
        {

            vehicleVariant.VariantId = Guid.NewGuid();

            vehicleService.AddVehicleVariant(vehicleVariant);
            Session["SuccessMessage"] = "Thêm thành công!";
            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult Edit(VehicleVariant vehicleVariant)
        {
            vehicleService.UpdateVehicleVariant(vehicleVariant);
            Session["SuccessMessage"] = "Cập nhật thành công!";
            return Redirect(Request.UrlReferrer.ToString());
        }


        [HttpPost]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult Delete(Guid id)
        {
            var VehicleVariant = vehicleService.GetVehicleVariant(id);

            if (VehicleVariant == null)
            {
                return HttpNotFound();
            }

            vehicleService.DeleteVehicleVariant(VehicleVariant);
            Session["SuccessMessage"] = "Xóa thành công!";
            return RedirectToAction("Index");
        }
    }
}