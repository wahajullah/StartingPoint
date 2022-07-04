using System;

namespace StartingPoint.Models.ServiceViewModel
{
    public class ServiceGridViewModel : EntityBase
    {
        public Int64 Id { get; set; }
        public string ServiceId { get; set; }
        public string ShortText { get; set; }
        public string Description { get; set; }
    }
}
