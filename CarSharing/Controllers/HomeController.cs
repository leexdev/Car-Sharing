using CarSharing.Helpers;
using CarSharing.Models;
using CarSharing.Service;
using System;
using System.Collections.Generic;
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
            _apiService = new ApiService();
        }
        public async Task<ActionResult> Index()
        {
            VehicleManagementModel objVehicleModel = vehicleService.GetHomeModel();
            ICollection<Review> reviews = vehicleService.GetReviews();

            Dictionary<Guid, double> ratingsByVehicleId = ReviewHelper.GetRatingsByVehicleId(reviews);
            ViewBag.RatingsByVehicleId = ratingsByVehicleId;
            return View(objVehicleModel);
        }
    }
}