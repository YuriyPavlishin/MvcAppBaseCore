using BaseApp.Web.Code.Infrastructure.Api.Swagger.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace BaseApp.Web.Code.Infrastructure.Api.Swagger
{
    public static class SwaggerExtensions
    {
        public static void AddAppWebSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
                {
                    options.DescribeAllEnumsAsStrings();
                    options.SwaggerDoc("v1", new Info { Title = "Web API", Version = "v1" });
                    options.OperationFilter<AuthorizationHeaderParameterOperationFilter>();
                    options.OperationFilter<FormFileOperationFilter>();

                    //Determine base path for the application.
                    var basePath = System.AppContext.BaseDirectory;
                    //Set the comments path for the swagger json and ui.
                    options.IncludeXmlComments(basePath + "\\BaseApp.Web.xml");

                });
        }

        public static void UseAppWebSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web API V1");
                }
            );
        }
    }
}
