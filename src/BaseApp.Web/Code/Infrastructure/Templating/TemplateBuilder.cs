using System.IO;
using RazorEngine;
using RazorEngine.Templating;

namespace BaseApp.Web.Code.Infrastructure.Templating
{
    public class TemplateBuilder: ITemplateBuilder
    {
        private readonly IPathResolver _PathResolver;

        public TemplateBuilder(IPathResolver pathResolver)
        {
            _PathResolver = pathResolver;
        }


        public string Render(string templateName, object model)
        {
            var viewFilePath = templateName.StartsWith("\\")
                    ? templateName
                    : "Views\\Templates\\" + templateName + ".cshtml";
            var physicPath = _PathResolver.MapPath(viewFilePath);
            
            if (!File.Exists(physicPath))
            {
                throw new FileNotFoundException("Template file not found - " + physicPath);
            }

            var result = Engine.Razor.RunCompile(File.ReadAllText(physicPath), physicPath, null, model);
            return result;
        }

    }
}