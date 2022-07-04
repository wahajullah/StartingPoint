using System;

namespace StartingPoint.Models
{
    public class MEPJobAdvice : EntityBase
    {
        public Int64 Id { get; set; } 
        public string MEPJobAdviceId { get; set; }
        public DateTime BookingDate { get; set; }
        public Int64 ServiceId { get; set; }
        public string ProjectName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Location { get; set; }
        public Int64 ClientId { get; set; }
        public Int64 ConsultantId { get; set; }
        public Int64 MContractorId { get; set; }
        public Int64 AgreementWithId { get; set; }
        public Int64 DivManagerId { get; set; }
        public Int64 TManagerId { get; set; }
        public Int64 PManagerId { get; set; }
        public Int64 SManagerId { get; set; }
        public Int64 PEACId { get; set; }
        public Int64 PEELId { get; set; }
        public Int64 PEPLId { get; set; }
        public Int64 PEFFId { get; set; }
        public Int64 PECIId { get; set; }
        public Int64 PEESId { get; set; }
        public Int64 PEELVId { get; set; }
        public Int64 PEFMId { get; set; }
        public Int64 PaymentTermId { get; set; }
        public string OCondition { get; set; }
        public string Insurance { get; set; }
    }
}
