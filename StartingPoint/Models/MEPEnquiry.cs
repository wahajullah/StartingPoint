using System;

namespace StartingPoint.Models
{
    public class MEPEnquiry : EntityBase
    {   
        
        public Int64 Id { get; set; }
        public string MEPEnquiryId { get; set; }
        public DateTime Date { get; set; }
        public Int64 ServiceId { get; set; }
        public string Project { get; set; }
        public Int64 MContractorId { get; set; }
        public Int64 ConsultantId { get; set; }
        public Int64 ClientId { get; set; }
        public DateTime DueDate { get; set; }
        public double? TenderPrice { get; set; }       
        public Int64 StatusId { get; set; }  
        public Int64 CityId { get; set; }   
        public Int64 CountryId { get; set; }
        public String Notes { get; set; }    

    }
}
