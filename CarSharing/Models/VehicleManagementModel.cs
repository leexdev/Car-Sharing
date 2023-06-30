using PagedList;
using System.Collections.Generic;

namespace CarSharing.Models
{
    public class VehicleManagementModel
    {
        public List<User> ListUsers { get; set; }
        public List<VehicleType> ListVehicleTypes { get; set; }
        public List<VehicleVariant> ListVehicleVariants { get; set; }
        public List<VehicleBrand> ListBrands { get; set; }
        public List<VehicleVariant> ListModels { get; set; }
        public List<Province> ListProvinces { get; set; }
        public List<District> ListDistricts { get; set; }
        public List<Vehicle> ListVehicles { get; set; }
        public List<Booking> ListBookings { get; set; }
        public List<Review> ListReviews { get; set; }
        public VehicleType VehicleType { get; set; }
        public VehicleVariant VehicleVariant { get; set; }
        public VehicleBrand VehicleBrand{ get; set; }
        public User User{ get; set; }
        public Vehicle Vehicle { get; set; }
        public Province Province { get; set; }
        public District District { get; set; }
        public Booking Booking { get; set; }
        public IPagedList<VehicleManagementModel> PagedVehicleTypes { get; set; }
        public IPagedList<VehicleManagementModel> PagedVehicleVariants { get; set; }
        public IPagedList<VehicleManagementModel> PagedVehicleBrands { get; set; }
        public IPagedList<VehicleManagementModel> PagedVehicles { get; set; }
        public IPagedList<VehicleManagementModel> PagedUsers { get; set; }

    }
}