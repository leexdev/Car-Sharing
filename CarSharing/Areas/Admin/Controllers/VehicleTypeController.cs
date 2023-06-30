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

namespace CarSharing.Areas.Admin.Controllers
{
    public class VehicleTypeController : Controller
    {
        private VehicleService vehicleService;

        public VehicleTypeController()
        {
            vehicleService = new VehicleService();
        }

        [CustomAuthorize(Roles = "admin")]
        public async Task<ActionResult> Index(string currentFilter, string searchString, int? page)
        {
            var objVehicleTypeModel = vehicleService.GetHomeModel();

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
                objVehicleTypeModel.ListVehicleTypes = vehicleService.GetVehicleTypesSearch(searchString).ToList();
            }

            ViewBag.CurrentFilter = searchString;
            int pageSize = 5;
            int pageNumber = page ?? 1;
            var pagedVehicleTypes = objVehicleTypeModel.ListVehicleTypes.ToPagedList(pageNumber, pageSize);
            var pagedVehicleTypeModels = new StaticPagedList<VehicleManagementModel>(
                pagedVehicleTypes.Select(a => new VehicleManagementModel
                {
                    ListVehicleTypes = new List<VehicleType> { a }
                }),
                pagedVehicleTypes.GetMetaData()
            );
            objVehicleTypeModel.PagedVehicleTypes = pagedVehicleTypeModels;
            return View(objVehicleTypeModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult Create(VehicleType vehicleType)
        {

            vehicleType.VehicleTypeId = Guid.NewGuid();

            vehicleService.AddVehicleType(vehicleType);
            Session["SuccessMessage"] = "Thêm thành công!";
            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult Edit(VehicleType vehicleType)
        {
            vehicleService.UpdateVehicleType(vehicleType);
            Session["SuccessMessage"] = "Cập nhật thành công!";
            return Redirect(Request.UrlReferrer.ToString());
        }


        [HttpPost]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult Delete(Guid id)
        {
            var vehicleType = vehicleService.GetVehicleType(id);

            if (vehicleType == null)
            {
                return HttpNotFound();
            }

            vehicleService.DeleteVehicleType(vehicleType);
            Session["SuccessMessage"] = "Xóa thành công!";
            return RedirectToAction("Index");
        }
    }
}