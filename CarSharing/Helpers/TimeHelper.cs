using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarSharing.Helpers
{
    public static class TimeHelper
    {
        public static string FormatTimeAgo(DateTime date)
        {
            TimeSpan timeAgo = DateTime.Now - date;

            if (timeAgo.TotalMinutes < 60)
            {
                return $"{Math.Max(1, (int)timeAgo.TotalMinutes)} phút trước";
            }
            else if (timeAgo.TotalHours < 24)
            {
                return $"{Math.Max(1, (int)timeAgo.TotalHours)} giờ trước";
            }
            else if (timeAgo.TotalDays < 30)
            {
                return $"{Math.Max(1, (int)timeAgo.TotalDays)} ngày trước";
            }
            else
            {
                return date.ToString("dd/MM/yyyy");
            }
        }
    }
}