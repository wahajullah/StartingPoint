using System;

namespace StartingPoint.Models.DepartmentViewModel
{
    public class DepartmentGridViewModel : EntityBase
    {
        public Int64 Id { get; set; }
        public string DepartmentId { get; set; }
        public string Description { get; set; }
    }
}
