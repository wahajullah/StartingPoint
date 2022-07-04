using System;

namespace StartingPoint.Models
{
    public class Status : EntityBase
    {
        public Int64 Id { get; set; }
        public string StatusId { get; set; }
        public string Description { get; set; }
        public int St { get; set; }
    }
}
