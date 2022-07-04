using System;

namespace StartingPoint.Models
{
    public class Country : EntityBase
    {
        public Int64 Id { get; set; }
        public string CountryId { get; set; }
        public string Description { get; set; }      
    }
}
