namespace BaseApp.Web.Code.Infrastructure.Templating
{
    public interface ITemplateBuilder
    {
        string Render(string templateName, object model);
    }
}
