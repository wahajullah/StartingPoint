using System;

namespace StartingPoint.Models.StatusViewModel
{
    public class StatusGridViewModel : EntityBase
    {
        public Int64 Id { get; set; }
        public string StatusId { get; set; }
        public string Description { get; set; }
    }
}
