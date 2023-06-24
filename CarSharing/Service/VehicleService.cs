using CarSharing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using CarSharing.Helpers;
using System.Data.Entity;

namespace CarSharing.Service
{
    public class VehicleService
    {
        private CarSharingDBEntities db;

        public VehicleService()
        {
            db = new CarSharingDBEntities();
        }

        public List<VehicleType> GetVehicleTypes()
        {
            return db.VehicleTypes.Where(vehicleType => vehicleType.isDeleted == false).ToList();
        }

        public List<VehicleBrand> GetBrands()
        {
            return db.VehicleBrands.Where(brand => brand.isDeleted == false).ToList();
        }
        public List<VehicleVariant> GetVariants()
        {
            return db.VehicleVariants.Where(variant => variant.isDeleted == false).ToList();
        }

        public List<Province> GetProvinces()
        {
            return db.Provinces.ToList();
        }
        public List<District> GetDistricts()
        {
            return db.Districts.ToList();
        }

        public List<Vehicle> GetVehicles()
        {
            return db.Vehicles.Where(vehicle => vehicle.isDeleted == false).ToList();
        }

        public Vehicle GetVehicle(Guid id)
        {
            return db.Vehicles.SingleOrDefault(article => article.VehicleId == id && !article.isDeleted);
        }

        public List<Review> GetReviews()
        {
            return db.Reviews.Where(review => review.isDeleted == false).ToList();
        }

        public VehicleManagementModel GetHomeModel()
        {
            VehicleManagementModel objHomeModel = new VehicleManagementModel();
            objHomeModel.ListVehicleTypes = GetVehicleTypes();
            objHomeModel.ListProvinces = GetProvinces();
            objHomeModel.ListDistricts = GetDistricts();
            objHomeModel.ListVehicles = GetVehicles();
            objHomeModel.ListReviews = GetReviews();
            return objHomeModel;
        }

        public VehicleManagementModel GetDetailModel()
        {
            VehicleManagementModel objDetailModel = new VehicleManagementModel();
            objDetailModel.ListVehicles = GetVehicles();
            objDetailModel.ListReviews = GetReviews();
            return objDetailModel;
        }

        public List<Vehicle> GetVehiclesSearch(string searchString)
        {
            diacriticsHelper diacriticsHelper = new diacriticsHelper();
            string normalizedSearchString = diacriticsHelper.RemoveDiacritics(searchString.ToUpper());

            return db.Vehicles
                .ToList()
                .Where(n => diacriticsHelper.RemoveDiacritics(n.VehicleName.ToUpper()).Contains(normalizedSearchString) && !n.isDeleted)
                .ToList();
        }

        public List<VehicleVariant> GetVariantsByVehicleType(Guid vehicleTypeId)
        {
                return db.VehicleVariants
                .ToList()
                .Where(v => v.VehicleTypeId == vehicleTypeId)
                .ToList();
        }
        public List<VehicleBrand> GetBrandsByVehicleType(Guid vehicleTypeId)
        {
            return db.VehicleBrands
            .ToList()
            .Where(v => v.VehicleTypeId == vehicleTypeId)
            .ToList();
        }

        public List<District> GetDistrictByProvince(int CodeProvince)
        {
            return db.Districts
            .ToList()
            .Where(v => v.code == CodeProvince)
            .ToList();
        }

        public void AddProvince(Province province)
        {
            db.Provinces.Add(province);
            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();
        }

        public void AddDistrict(District district)
        {
            db.Districts.Add(district);
            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();
        }

        public void AddVehicle(Vehicle vehicle)
        {
            db.Vehicles.Add(vehicle);
            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();
        }

        public Province GetProvinceByCode(int provinceCode)
        {
            var province = db.Provinces.FirstOrDefault(p => p.Code == provinceCode);
            return province;
        }

        public District GetDistrictByCode(int districtCode)
        {
            var district = db.Districts.FirstOrDefault(p => p.code == districtCode);
            return district;
        }
    }
}