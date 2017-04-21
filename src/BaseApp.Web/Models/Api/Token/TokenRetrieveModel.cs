using System;

namespace BaseApp.Web.Models.Api.Token
{
    public class TokenRetrieveModel
    {
        public bool IsAuthenticated { get; set; }
        public string Token { get; set; }
        public DateTime? TokenExpiresAt { get; set; }
    }
}
