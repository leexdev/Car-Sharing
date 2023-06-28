using CarSharing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using CarSharing.Helpers;
using System.Data.Entity;
using System.ComponentModel;

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
            return db.VehicleTypes.Where(vehicleType => !vehicleType.isDeleted).ToList();
        }

        public List<VehicleBrand> GetBrands()
        {
            return db.VehicleBrands.Where(brand => !brand.isDeleted).ToList();
        }
        public List<VehicleVariant> GetVariants()
        {
            return db.VehicleVariants.Where(variant => !variant.isDeleted).ToList();
        }

        public List<Province> GetProvinces()
        {
            var provincesWithVehicles = db.Districts
                .Where(d => d.Vehicles.Any(v => !v.isDeleted))
                .Select(d => d.Province)
                .Distinct()
                .ToList();

            return provincesWithVehicles;
        }

        public List<District> GetDistricts()
        {
            return db.Districts.ToList();
        }

        public List<District> GetDistrictsByProvince(int provinceId)
        {
            return db.Districts
                .Where(d => d.province_code == provinceId && d.Vehicles.Any(v => v.isDeleted == false))
                .ToList();
        }

        public List<Vehicle> GetVehicles()
        {
            return db.Vehicles.Where(vehicle => !vehicle.isDeleted).ToList();
        }

        public List<Vehicle> SearchVehicles(Guid vehicleType, int provinceId)
        {
            return db.Vehicles.Where(vehicle => vehicle.VehicleBrand.VehicleTypeId == vehicleType && vehicle.District.province_code == provinceId && !vehicle.isDeleted).ToList();
        }

        public VehicleType GetVehicleType(Guid id)
        {
            return db.VehicleTypes.SingleOrDefault(vehicleType => vehicleType.VehicleTypeId == id && !vehicleType.isDeleted);
        }

        public VehicleVariant GetVehicleVariant(Guid id)
        {
            return db.VehicleVariants.SingleOrDefault(vehicleVariant => vehicleVariant.VariantId == id && !vehicleVariant.isDeleted);
        }

        public VehicleBrand GetVehicleBrand(Guid id)
        {
            return db.VehicleBrands.SingleOrDefault(vehicleBrand => vehicleBrand.BrandId == id && !vehicleBrand.isDeleted);
        }

        public Vehicle GetVehicle(Guid id)
        {
            return db.Vehicles.SingleOrDefault(vehicle => vehicle.VehicleId == id && !vehicle.isDeleted);
        }

        public List<Review> GetReviews()
        {
            return db.Reviews.Where(review => !review.isDeleted).ToList();
        }

        public VehicleManagementModel GetHomeModel()
        {
            VehicleManagementModel objHomeModel = new VehicleManagementModel();
            objHomeModel.ListVehicleTypes = GetVehicleTypes();
            objHomeModel.ListVehicleVariants = GetVariants();
            objHomeModel.ListBrands = GetBrands();
            objHomeModel.ListProvinces = GetProvinces();
            objHomeModel.ListDistricts = GetDistricts();
            objHomeModel.ListVehicles = GetVehicles();
            objHomeModel.ListReviews = GetReviews();
            return objHomeModel;
        }

        public VehicleManagementModel GetVariantModel()
        {
            VehicleManagementModel objVariantModel = new VehicleManagementModel();
            objVariantModel.ListVehicleTypes = GetVehicleTypes();
            objVariantModel.ListVehicleVariants = GetVariants();
            return objVariantModel;
        }

        public VehicleManagementModel GetBrandModel()
        {
            VehicleManagementModel objBrandModel = new VehicleManagementModel();
            objBrandModel.ListVehicleTypes = GetVehicleTypes();
            objBrandModel.ListBrands = GetBrands();
            return objBrandModel;
        }

        public VehicleManagementModel GetDetailModel()
        {
            VehicleManagementModel objDetailModel = new VehicleManagementModel();
            objDetailModel.ListVehicles = GetVehicles();
            objDetailModel.ListReviews = GetReviews();
            return objDetailModel;
        }

        public List<VehicleType> GetVehicleTypesSearch(string searchString)
        {
            diacriticsHelper diacriticsHelper = new diacriticsHelper();
            string normalizedSearchString = diacriticsHelper.RemoveDiacritics(searchString.ToUpper());

            return db.VehicleTypes
                .ToList()
                .Where(n => diacriticsHelper.RemoveDiacritics(n.VehicleTypeName.ToUpper()).Contains(normalizedSearchString) && !n.isDeleted)
                .ToList();
        }

        public List<VehicleVariant> GetVehicleVariantsSearch(string searchString)
        {
            diacriticsHelper diacriticsHelper = new diacriticsHelper();
            string normalizedSearchString = diacriticsHelper.RemoveDiacritics(searchString.ToUpper());

            return db.VehicleVariants
                .ToList()
                .Where(n => diacriticsHelper.RemoveDiacritics(n.VariantName.ToUpper()).Contains(normalizedSearchString) && !n.isDeleted)
                .ToList();
        }

        public List<VehicleBrand> GetVehicleBrandsSearch(string searchString)
        {
            diacriticsHelper diacriticsHelper = new diacriticsHelper();
            string normalizedSearchString = diacriticsHelper.RemoveDiacritics(searchString.ToUpper());

            return db.VehicleBrands
                .ToList()
                .Where(n => diacriticsHelper.RemoveDiacritics(n.BrandName.ToUpper()).Contains(normalizedSearchString) && !n.isDeleted)
                .ToList();
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
                .Where(v => v.VehicleTypeId == vehicleTypeId && !v.isDeleted)
                .ToList();
        }

        public List<VehicleBrand> GetBrandsByVehicleType(Guid vehicleTypeId)
        {
            return db.VehicleBrands
                .ToList()
                .Where(v => v.VehicleTypeId == vehicleTypeId && !v.isDeleted)
                .ToList();
        }

        public List<Province> GetProvinceByVehicleType(Guid vehicleTypeId)
        {
            return db.Provinces
                .Where(p => p.Districts.Any(d => d.Vehicles.Any(v => v.VehicleVariant.VehicleTypeId == vehicleTypeId && !v.isDeleted)))
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
        
        public void AddVehicleType(VehicleType vehicleType)
        {
            db.VehicleTypes.Add(vehicleType);
            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();
        }

        public void UpdateVehicleType(VehicleType vehicleType)
        {
            var existingVehicleType = db.VehicleTypes.Find(vehicleType.VehicleTypeId);

            if (existingVehicleType != null)
            {
                existingVehicleType.VehicleTypeName = vehicleType.VehicleTypeName;

                db.SaveChanges();
            }
        }

        public void DeleteVehicleType(VehicleType vehicleType)
        {
            vehicleType.isDeleted = true;
            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();
        }

        public void AddVehicleVariant(VehicleVariant vehicleVariant)
        {
            db.VehicleVariants.Add(vehicleVariant);
            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();
        }

        public void UpdateVehicleVariant(VehicleVariant vehicleVariant)
        {
            var existingVehicleVariant = db.VehicleVariants.Find(vehicleVariant.VariantId);

            if (existingVehicleVariant != null)
            {
                existingVehicleVariant.VehicleTypeId = vehicleVariant.VehicleTypeId;
                existingVehicleVariant.VariantName = vehicleVariant.VariantName;

                db.SaveChanges();
            }
        }

        public void DeleteVehicleVariant(VehicleVariant vehicleVariant)
        {
            vehicleVariant.isDeleted = true;
            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();
        }

        public void AddVehicleBrand(VehicleBrand vehicleBrand)
        {
            db.VehicleBrands.Add(vehicleBrand);
            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();
        }

        public void UpdateVehicleBrand(VehicleBrand vehicleBrand)
        {
            var existingVehicleBrand = db.VehicleBrands.Find(vehicleBrand.BrandId);

            if (existingVehicleBrand != null)
            {
                existingVehicleBrand.VehicleTypeId = vehicleBrand.VehicleTypeId;
                existingVehicleBrand.BrandName = vehicleBrand.BrandName;

                db.SaveChanges();
            }
        }

        public void DeleteVehicleBrand(VehicleBrand vehicleBrand)
        {
            vehicleBrand.isDeleted = true;
            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();
        }

        public void AddVehicle(Vehicle vehicle)
        {
            db.Vehicles.Add(vehicle);
            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();
        }

        public void UpdateVehicle(Vehicle vehicle, District district)
        {
            var existingVehicle = db.Vehicles.Find(vehicle.VehicleId);

            if (existingVehicle != null)
            {
                existingVehicle.VehicleName = vehicle.VehicleName;
                existingVehicle.Content = vehicle.Content;
                existingVehicle.VariantId = vehicle.VariantId;
                existingVehicle.BrandId = vehicle.BrandId;
                existingVehicle.ImageVehicle = vehicle.ImageVehicle;
                existingVehicle.Year = vehicle.Year;
                existingVehicle.RegistrationNumber = vehicle.RegistrationNumber;
                existingVehicle.VehiclePrice = vehicle.VehiclePrice;
                existingVehicle.CodeDistrict = district.code;

                db.SaveChanges();
            }
        }

        public void DeleteVehicle(Vehicle vehicle)
        {
            vehicle.isDeleted = true;
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