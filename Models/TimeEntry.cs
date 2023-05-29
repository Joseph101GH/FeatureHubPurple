using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeatureHubPurple.Models
{
    public class TimeEntry
    {
        public string Time
        {
            get; set;
        }
        public int TotalMinutes
        {
            get; set;
        }
        public bool IsActive
        {
            get; set;
        }
    }

}
