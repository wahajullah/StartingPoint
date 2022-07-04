using System;

namespace StartingPoint.Models
{
    public class Service : EntityBase
    {
        public Int64 Id { get; set; }
        public string ServiceId { get; set; }
        public int DivisionID { get; set; }
        public string ShortText { get; set; }
        public string Description { get; set; }      
    }
}
