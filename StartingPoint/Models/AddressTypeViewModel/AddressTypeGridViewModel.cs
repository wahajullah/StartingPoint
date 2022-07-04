using System;

namespace StartingPoint.Models.AddressTypeViewModel
{
    public class AddressTypeGridViewModel : EntityBase
    {
        public Int64 Id { get; set; }
        public string AddressTypeId { get; set; }
        public string Description { get; set; }
    }
}
