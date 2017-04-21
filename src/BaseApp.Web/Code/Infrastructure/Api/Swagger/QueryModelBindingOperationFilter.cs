using Swashbuckle.Swagger.Model;
using Swashbuckle.SwaggerGen;
using Swashbuckle.SwaggerGen.Generator;

namespace BaseApp.Web.Code.Infrastructure.Api.Swagger
{
    public class QueryModelBindingOperationFilter: IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                return;

            foreach (IParameter param in operation.Parameters)
            {
                if (param.In == "modelbinding")
                    param.In = "query";
            }
        }
    }
}