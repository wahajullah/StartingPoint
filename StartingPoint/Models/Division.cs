using System;

namespace StartingPoint.Models
{
    public class Division : EntityBase
    {
        public Int64 Id { get; set; }
        public string DivisionId { get; set; }
        public string ShortText { get; set; }
        public string Description { get; set; }      
    }
}
