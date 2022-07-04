using System;

namespace StartingPoint.Models
{
    public class AddressType : EntityBase
    {
        public Int64 Id { get; set; }
        public string AddressTypeId { get; set; }
        public string Description { get; set; }      
    }
}
