namespace BaseApp.Web.Code.Infrastructure.LogOn
{
    public class LoggedUserDbInfo
    {
        public string Login { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public long GeneratedDateTicks { get; }

        public string FullName => $"{FirstName} {LastName}".Trim();

        public LoggedUserDbInfo(string login, string firstName, string lastName, long generatedDateTicks)
        {
            Login = login;
            FirstName = firstName;
            LastName = lastName;
            GeneratedDateTicks = generatedDateTicks;
        }
        
        public static string GetUserDbInfoCacheKey(string login)
        {
            return "userLogon_" + login;
        }
    }
}
