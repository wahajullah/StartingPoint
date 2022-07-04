using System;

namespace StartingPoint.Models.MaterialGroupViewModel
{
    public class MaterialGroupGridViewModel : EntityBase
    {
        public Int64 Id { get; set; }
        public string MaterialGroupId { get; set; }
        public Int64 ServiceId { get; set; }
        public string Description { get; set; }
    }
}
