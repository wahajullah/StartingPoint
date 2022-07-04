using StartingPoint.Pages;
using System;

namespace StartingPoint.Helpers
{
    public static class StaticData
    {
        public static string RandomDigits(int length)
        {
            var random = new Random();
            string s = string.Empty;
            for (int i = 0; i < length; i++)
                s = String.Concat(s, random.Next(10).ToString());
            return s;
        }
        public static string GetUniqueID(string Prefix)
        {
            Random _Random = new Random();
            var result = Prefix + DateTime.Now.ToString("yyyyMMddHHmmss") + _Random.Next(1, 1000);
            return result;
        }
    }
    public static class AssetStatusValue
    {
        public const int New = 1;
        public const int InUse = 2;
        public const int Available = 3;
        public const int Damage = 4;
        public const int Return = 5;
        public const int Expired = 6;
        public const int RequiredLicenseUpdate = 7;
        public const int Miscellaneous = 8;
    }

    public static class DefaultUserPage
    {
        public static readonly string[] PageCollection =
            {
                MainMenu.Dashboard.PageName,
                MainMenu.UserProfile.PageName,
            };
    }
}
