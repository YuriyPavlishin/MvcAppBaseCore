using System.Collections.Generic;

namespace BaseApp.Data.DataContext.Projections.Users
{
    public class AccountProjection
    {
        public AccountProjection()
        {
            Roles = new string[0];
        }

        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}
