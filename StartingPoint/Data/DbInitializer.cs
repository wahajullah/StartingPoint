using System.Linq;
using System.Threading.Tasks;
using StartingPoint.Services;

namespace StartingPoint.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(ApplicationDbContext context, IFunctional functional)
        {
            context.Database.EnsureCreated();
            await functional.GetDefaultIdentitySettings();

            if (context.ApplicationUser.Any())
            {
                return;
            }
            else
            {
                await functional.CreateDefaultSuperAdmin();
                await functional.CreateDefaultEmailSettings();
                await functional.CreateDefaultIdentitySettings();
                //await functional.CreateAsset();
                functional.InitAppData();
            }
        }
    }
}
