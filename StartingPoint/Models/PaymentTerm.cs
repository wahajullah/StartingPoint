using System;

namespace StartingPoint.Models
{
    public class PaymentTerm : EntityBase
    {
        public Int64 Id { get; set; }
        public string PaymentTermId { get; set; }
        public string Description { get; set; }      
    }
}
