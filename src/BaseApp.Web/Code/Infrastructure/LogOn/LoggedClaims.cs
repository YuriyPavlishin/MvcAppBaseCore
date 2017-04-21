using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using BaseApp.Data.DataContext.Projections.Users;

namespace BaseApp.Web.Code.Infrastructure.LogOn
{
    public class LoggedClaims
    {
        private const string ClaimTypesUserId = "UserId";
        private const string ClaimTypesGeneratedDate = "GeneratedDateTicks";

        public int UserId { get; }
        public string Login { get; }
        public IEnumerable<string> Roles { get; }
        public long GeneratedDateTicks { get; }

        public LoggedClaims(AccountProjection account)
            :this(account.Id, account.Login, account.Roles)
        {
            
        }

        public LoggedClaims(int userId, string login, IEnumerable<string> roles)
        {
            UserId = userId;
            Login = login;
            Roles = roles;
            GeneratedDateTicks = DateTime.UtcNow.Ticks;
        }

        public LoggedClaims(List<Claim> claims)
        {
            UserId = int.Parse(claims.Single(x => x.Type == ClaimTypesUserId).Value);
            Login = claims.Single(x => x.Type == ClaimTypes.Name).Value;
            Roles = claims.Single(x => x.Type == ClaimTypes.Role).Value.Split(",".ToCharArray());
            GeneratedDateTicks = long.Parse(claims.Single(x => x.Type == ClaimTypesGeneratedDate).Value);
        }

        public List<Claim> GetAsClaims()
        {
            return new List<Claim>()
                   {
                       new Claim(ClaimTypesUserId, UserId.ToString()),
                       new Claim(ClaimTypes.Name, Login),
                       new Claim(ClaimTypes.Role, string.Join(",", Roles)),
                       new Claim(ClaimTypesGeneratedDate, GeneratedDateTicks.ToString())
                   };
        }
    }
}
