using System;

namespace StartingPoint.Models
{
    public class City : EntityBase
    {
        public Int64 Id { get; set; }
        public string CityID { get; set; }
        public string Name { get; set; }      
    }
}
