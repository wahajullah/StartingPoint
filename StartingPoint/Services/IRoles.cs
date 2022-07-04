using StartingPoint.Pages;
using System.Threading.Tasks;

namespace StartingPoint.Services
{
    public interface IRoles
    {
        Task GenerateRolesFromPagesAsync();

        Task AddToRoles(string applicationUserId);
        Task<MainMenuViewModel> RolebaseMenuLoad(string applicationUserId);
    }
}
