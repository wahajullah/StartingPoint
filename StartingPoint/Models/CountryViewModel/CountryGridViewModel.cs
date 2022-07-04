using System;

namespace StartingPoint.Models.CountryViewModel
{
    public class CountryGridViewModel : EntityBase
    {
        public Int64 Id { get; set; }
        public string CountryId { get; set; }
        public string Description { get; set; }
    }
}
