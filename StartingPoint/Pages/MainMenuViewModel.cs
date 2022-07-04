namespace StartingPoint.Pages
{
    public class MainMenuViewModel
    {
        public bool Admin { get; set; }
        public bool Dashboard { get; set; }
        public bool UserManagement { get; set; }
        public bool UserProfile { get; set; }
        public bool ManagePageAccess { get; set; }
        public bool EmailSetting { get; set; }
        public bool IdentitySetting { get; set; }
        public bool LoginHistory { get; set; }

        //StartingPoint
        
        public bool CompanyInfo { get; set; }


        //AddressBook
        public bool AddressBookDetails { get; set; }
        public bool AddressBook { get; set; }
        public bool AddressType { get; set; }

        //General Settings
        public bool City { get; set; }
        public bool Country { get; set; }   
        public bool Status { get; set; }
        public bool Service { get; set; }    
        public bool PaymentTerm { get; set; }
        public bool MEPDivision { get; set; }       
        public bool MEPEstimation { get; set; }
        public bool MEPProjects { get; set; }
        public bool CentralStore { get; set; }
        public bool MaterialGroup { get; set; }
        public bool Material { get; set; }
        public bool MEPPurchase { get; set; }       
       
        public bool FMDivision { get; set; }
        public bool FireDivision { get; set; }
        public bool ESCODivision { get; set; }
        public bool ELVDivision { get; set; }
        public bool GeneralDepartments { get; set; }
        public bool GeneralSettings { get; set; }
        public bool Designation { get; set; }
        public bool Department { get; set; }
        public bool Division { get; set; }
    }
}