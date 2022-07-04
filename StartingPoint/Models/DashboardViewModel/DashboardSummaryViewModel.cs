using System;
using System.Collections.Generic;


namespace StartingPoint.Models.DashboardViewModel
{
    public class DashboardSummaryViewModel
    {
        public Int64 TotalUser { get; set; }    
        public Int64 TotalActive { get; set; }
        public Int64 TotalInActive { get; set; } 
        public List<UserProfile> listUserProfile { get; set; }
        public Int64 TotalAsset { get; set; }
        public Int64 TotalAssignedAsset { get; set; }
        public Int64 TotalUnAssignedAsset { get; set; }
        public Int64 TotalEmployee { get; set; }    
        
    }
}
