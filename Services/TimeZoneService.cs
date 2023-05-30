using System;
using System.Collections.Generic;
using FeatureHubPurple.Models;

namespace FeatureHubPurple.Services
{
    public class TimeZoneService
    {
        // List of time zone IDs
        private readonly List<string> _timeZoneIds = new List<string>
        {
        "Eastern Standard Time",
        "Central Standard Time",
        "Mountain Standard Time",
        "Pacific Standard Time",
        "GMT Standard Time", // British Summer Time
        "Central Europe Standard Time", // Central European Summer Time
        "E. Europe Standard Time", // Eastern European Summer Time
        "AUS Eastern Standard Time" // Australian Eastern Standard Time
        };

        /// <summary>
        /// Gets a list of time zones with their current times.
        /// </summary>
        /// <returns>A list of strings, each representing a time zone and its current time.</returns>
        public List<TimeZoneInfoModel> GetTimeZonesWithCurrentTimes()
        {
            var formattedTimeZones = new List<TimeZoneInfoModel>();

            foreach (var timeZoneId in _timeZoneIds)
            {
                var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                var currentTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, timeZoneInfo);
                formattedTimeZones.Add(new TimeZoneInfoModel { TimeZoneName = timeZoneId, CurrentTime = currentTime });
            }

            return formattedTimeZones;
        }
    }
}
