using System;

namespace StartingPoint.Models
{
    public class Department : EntityBase
    {
        public Int64 Id { get; set; }
        public string DepartmentId { get; set; }
        public string Description { get; set; }      
    }
}
