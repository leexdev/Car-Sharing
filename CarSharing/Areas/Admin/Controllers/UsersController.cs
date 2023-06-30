using CarSharing.Models;
using CarSharing.Service;
using PagedList;
using System;
using System.Collections.Generic;
using System.EnterpriseServices.CompensatingResourceManager;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CarSharing.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        private readonly VehicleService vehicleService;

        public UsersController()
        {
            vehicleService = new VehicleService();
        }

        [CustomAuthorize(Roles = "admin")]
        public async Task<ActionResult> Index(string currentFilter, string searchString, int? page)
        {
            var objUserModel = vehicleService.GetUserModel();

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
                objUserModel.ListUsers = vehicleService.GetUsersSearch(searchString).ToList();
            }

            ViewBag.CurrentFilter = searchString;
            int pageSize = 5;
            int pageNumber = page ?? 1;
            var pagedUsers = objUserModel.ListUsers.ToPagedList(pageNumber, pageSize);
            var pagedUserModels = new StaticPagedList<VehicleManagementModel>(
                pagedUsers.Select(a => new VehicleManagementModel
                {
                    ListUsers = new List<User> { a }
                }),
                pagedUsers.GetMetaData()
            );
            objUserModel.PagedUsers = pagedUserModels;
            return View(objUserModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult Create(User user, HttpPostedFileBase ImageUpLoad)
        {
            user.UserId = Guid.NewGuid();
            user.Avatar = "/Content/assets/img/user.png";
            user.Password = "e10adc3949ba59abbe56e057f20f883e";
            vehicleService.AddUser(user);
            Session["SuccessMessage"] = "Thêm thành công!";
            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult CheckEmailExists(string email)
        {
            var emailExists = vehicleService.CheckEmailExists(email);

            if (emailExists != null)
            {
                return Json(new { success = false, message = "Email đã tồn tại." });
            }
            else
            {
                return Json(new { success = true });
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [CustomAuthorize(Roles = "admin")]
        //[CustomAuthorize(Roles = "admin")]
        public ActionResult Edit(User user)
        {
            vehicleService.UpdateUser(user);
            Session["SuccessMessage"] = "Cập nhật thành công!";
            return Redirect(Request.UrlReferrer.ToString());
        }


        [HttpPost]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult Delete(Guid id)
        {
            var user = vehicleService.GetUser(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            vehicleService.DeleteUser(user);
            Session["SuccessMessage"] = "Xóa thành công!";
            return RedirectToAction("Index");
        }


        [CustomAuthorize(Roles = "admin")]
        public async Task<ActionResult> Partner(string currentFilter, string searchString, int? page)
        {
            var objUserModel = vehicleService.GetUserModel();

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
                objUserModel.ListUsers = vehicleService.GetUsersSearch(searchString).ToList();
            }
            var listUser = objUserModel.ListUsers.Where(u => u.PartnerRequest).ToList();

            objUserModel.ListUsers = listUser.ToList();

            ViewBag.CurrentFilter = searchString;
            int pageSize = 5;
            int pageNumber = page ?? 1;
            var pagedUsers = objUserModel.ListUsers.ToPagedList(pageNumber, pageSize);
            var pagedUserModels = new StaticPagedList<VehicleManagementModel>(
                pagedUsers.Select(a => new VehicleManagementModel
                {
                    ListUsers = new List<User> { a }
                }),
                pagedUsers.GetMetaData()
            );
            objUserModel.PagedUsers = pagedUserModels;
            return View(objUserModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult AddPartner(User user)
        {
            var partner = vehicleService.GetUser(user.UserId);
            user.Role = "partner";
            user.PartnerRequest = false;
            vehicleService.UpdatePartner(user);
            return RedirectToAction("Partner");
        }

        [HttpPost]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult DeletePartner(Guid id)
        {
            var user = vehicleService.GetUser(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            vehicleService.DeletePartner(user);
            Session["SuccessMessage"] = "Xóa thành công!";
            return RedirectToAction("Partner");
        }
    }
}