using StartingPoint.Helpers;
using StartingPoint.Models;
using System;
using System.Collections.Generic;

namespace StartingPoint.Data
{
    public class SeedData
    {
        
        public CompanyInfo GetCompanyInfo()
        {
            return new CompanyInfo
            {
                Name = "XYZ Company Limited",
                Logo = "/upload/company_logo.png",
                Currency = "৳",
                Address = "Dhaka, Bangladesh",
                City = "Dhaka",
                Country = "Bangladesh",
                Phone = "132546789",
                Fax = "9999",
                Website = "www.wyx.com",
            };
        }

      
    }
}
