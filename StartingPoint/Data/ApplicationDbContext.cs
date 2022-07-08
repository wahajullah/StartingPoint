using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StartingPoint.Models;

namespace StartingPoint.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<UserProfile> UserProfile { get; set; }

        public DbSet<SMTPEmailSetting> SMTPEmailSetting { get; set; }
        public DbSet<SendGridSetting> SendGridSetting { get; set; }
        public DbSet<DefaultIdentityOptions> DefaultIdentityOptions { get; set; }
        public DbSet<LoginHistory> LoginHistory { get; set; }

        //StartingPoint        
       
        public DbSet<CompanyInfo> CompanyInfo { get; set; }

        //Khamer
        
        public DbSet<City> Cities { get; set; }
        public DbSet<AddressBook> AddressBooks { get; set; }
        public DbSet<AddressType> AddressTypes { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Country> Countries { get; set; }       
        public DbSet<MEPEnquiry> MEPEnquiries { get; set; }
        public DbSet<MaterialGroup> MaterialGroups { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<PaymentTerm> PaymentTerms { get; set; }
        public DbSet<Designation> Designations { get; set; }
        public DbSet<Department> Departments { get; set; }
        //testing
        public DbSet<Student> Students { get; set; }
        public DbSet<Grade> Grades { get; set; }

        public DbSet<Division> Divisions { get; set; }

        public DbSet<Groups> Groups { get; set; }
        public DbSet<LineItems> LineItems { get; set; }

        public DbSet<Service> Services { get; set; }
        public DbSet<Enquiry> Enquiry { get; set; }
    }
}
