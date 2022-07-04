using System;

namespace StartingPoint.Models
{
    public class AddressBook : EntityBase
    {        
        public Int64 Id { get; set; }
        public string AddressId { get; set; }
        public string Name { get; set; }
        public string JobTitle { get; set; }
        public string PPhone { get; set; }
        public string Mobile { get; set; }
        public string PEmail { get; set; }
        public string Company { get; set; }
        public string OEmail { get; set; }
        public string OPhone { get; set; }
        public string OFax { get; set; }
        public string Website { get; set; }
        public string Pobox { get; set; }
        public Int64 AddressTypeId { get; set; }
        public Int64 CityId { get; set; }
        public Int64 CountryId { get; set; }
        public Int64 StatusId { get; set; }
        public string Address { get; set; }         
    }
}
