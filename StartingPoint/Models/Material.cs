using System;

namespace StartingPoint.Models
{
    public class Material : EntityBase
    {
        public Int64 Id { get; set; } 
        public string MaterialId { get; set; }
        public Int64 ServiceId { get; set; }
        public Int64 MaterialGroupId { get; set; }
        public string Description { get; set; } 
        public string Unit { get; set; }
        public double? Rate { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public Int64 SupplierId { get; set; }
        public string Size { get; set; }
        public string Weight { get; set; }
        public Int64 CountryId { get; set; }        
    }
}
