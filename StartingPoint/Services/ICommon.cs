using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using UAParser;
using StartingPoint.Models;
using StartingPoint.Models.CommonViewModel;
using System.Linq;
using StartingPoint.Models.UserAccountViewModel;
using StartingPoint.Models.CompanyInfoViewModel;



namespace StartingPoint.Services
{
    public interface ICommon
    {
        string UploadedFile(IFormFile ProfilePicture);
        Task<SMTPEmailSetting> GetSMTPEmailSetting();
        Task<SendGridSetting> GetSendGridEmailSetting();
        UserProfile GetByUserProfile(Int64 id);
        UserProfileCRUDViewModel GetByUserProfileInfo(Int64 id);
        bool InsertLoginHistory(LoginHistory _LoginHistory, ClientInfo _ClientInfo);
          
     
        CompanyInfoCRUDViewModel GetCompanyInfo();


        //khamer
        IQueryable<ItemDropdownListViewModel> LoadddlCity();
        IQueryable<ItemDropdownListViewModel> LoadddlAddressType();
        IQueryable<ItemDropdownListViewModel> LoadddlStatus();
        IQueryable<ItemDropdownListViewModel> LoadddlCountry();
        IQueryable<ItemDropdownListViewModel> LoadddlService();
        IQueryable<ItemDropdownListViewModel> LoadddlDivision();
        IQueryable<ItemDropdownListViewModel> LoadddlGroup();
        IQueryable<ItemDropdownListViewModel> LoadddlMaterialGroup();
        IQueryable<ItemDropdownListViewModel> LoadddlAddressSupplier();
    }
}
