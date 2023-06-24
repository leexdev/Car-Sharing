using PagedList;
using System.Collections.Generic;

namespace CarSharing.Models
{
    public class VehicleManagementModel
    {
        public List<VehicleType> ListVehicleTypes { get; set; }
        public List<VehicleBrand> ListBrands { get; set; }
        public List<VehicleVariant> ListModels { get; set; }
        public List<Province> ListProvinces { get; set; }
        public List<District> ListDistricts { get; set; }
        public List<Vehicle> ListVehicles { get; set; }
        public List<Review> ListReviews { get; set; }
        public Vehicle Vehicle { get; set; }
        public Province Province { get; set; }
        public District District { get; set; }
        public IPagedList<VehicleManagementModel> PagedVehicles { get; set; }

    }
}