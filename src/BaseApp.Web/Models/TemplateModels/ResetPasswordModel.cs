namespace BaseApp.Web.Models.TemplateModels
{
    public class ResetPasswordModel
    {
        public string UserName { get; set; }
        public string ResetPasswordUrl { get; set; }
        public string RequestIp { get; set; }
    }
}
