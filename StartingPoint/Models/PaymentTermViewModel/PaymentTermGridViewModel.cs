using System;

namespace StartingPoint.Models.PaymentTermViewModel
{
    public class PaymentTermGridViewModel : EntityBase
    {
        public Int64 Id { get; set; }
        public string PaymentTermId { get; set; }
        public string Description { get; set; }
    }
}
