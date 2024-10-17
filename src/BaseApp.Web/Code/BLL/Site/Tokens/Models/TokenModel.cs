using System;

namespace BaseApp.Web.Code.BLL.Site.Tokens.Models
{
    public class TokenModel
    {
        public bool IsAuthenticated { get; set; }
        public string Token { get; set; }
        public DateTime? TokenExpiresAt { get; set; }
    }
}
