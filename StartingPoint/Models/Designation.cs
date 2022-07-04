using System;

namespace StartingPoint.Models
{
    public class Designation : EntityBase
    {
        public Int64 Id { get; set; }
        public string DesignationId { get; set; }
        public string Description { get; set; }      
    }
}
