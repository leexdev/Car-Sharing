using CarSharing.Models;
using CarSharing.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarSharing.Controllers
{
    public class PayController : Controller
    {
        private readonly VehicleService vehicleService;

        public PayController()
        {
            vehicleService = new VehicleService();
        }

        [CustomAuthorize(Roles = "admin, partner, user")]
        public ActionResult Index(Guid id, DateTime startTime, DateTime endTime)
        {
            var vehicle = vehicleService.GetVehicle(id);
            var userId = new Guid(Session["Id"].ToString());
            var user = vehicleService.GetUser(userId);

            TimeSpan duration = (endTime - startTime);
            int totalDays = (int)Math.Abs(duration.TotalDays) + 1;
            ViewBag.TotalDays = totalDays;
            ViewBag.StartTime = startTime;
            ViewBag.EndTime = endTime;

            Session["StartTime"] = startTime;
            Session["EndTime"] = endTime;

            decimal totalPrice = totalDays * vehicle.VehiclePrice;

            VehicleManagementModel objVehicleModel = new VehicleManagementModel();
            objVehicleModel.Vehicle = vehicle;
            objVehicleModel.User = user;
            objVehicleModel.Booking = new Booking(); // Khởi tạo Booking nếu cần thiết
            objVehicleModel.Booking.TotalPrice = totalPrice;

            return View(objVehicleModel);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "admin, partner, user")]
        public ActionResult Pay(Booking booking, Guid vehicleId)
        {
            booking.BookingId = Guid.NewGuid();
            booking.VehicleId = vehicleId;
            booking.UserId = new Guid(Session["Id"].ToString());
            booking.Email = (Session["Email"].ToString());
            booking.StartTime = (DateTime)Session["StartTime"];
            booking.EndTime = (DateTime)Session["EndTime"];
            booking.Status = "Chờ duyệt";
            
            vehicleService.AddBooking(booking);
            return View("PaySuccess");
        }

        public ActionResult PaySuccess()
        {
            return View();
        }
    }
}