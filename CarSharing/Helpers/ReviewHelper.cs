using CarSharing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarSharing.Helpers
{
    public static class ReviewHelper
    {
        public static Dictionary<Guid, double> GetRatingsByVehicleId(ICollection<Review> reviews)
        {
            Dictionary<Guid, double> ratingsByVehicleId = new Dictionary<Guid, double>();

            if (reviews != null)
            {
                foreach (var review in reviews)
                {
                    if (ratingsByVehicleId.ContainsKey(review.VehicleId))
                    {
                        ratingsByVehicleId[review.VehicleId] += review.Rating;
                    }
                    else
                    {
                        ratingsByVehicleId[review.VehicleId] = review.Rating;
                    }
                }
            }

            return ratingsByVehicleId;
        }
    }

}