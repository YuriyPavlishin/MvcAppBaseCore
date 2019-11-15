using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;

namespace BaseApp.Web.Code.Infrastructure.Api.Swagger
{
    public static class SwaggerExtensions
    {
        public static void AddAppWebSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
                {
                    //options.DescribeAllEnumsAsStrings();
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Web API", Version = "v1" });

                    //Determine base path for the application.
                    var basePath = System.AppContext.BaseDirectory;
                    //Set the comments path for the swagger json and ui.
                    options.IncludeXmlComments(basePath + "\\BaseApp.Web.xml");

                    options.AddSecurityDefinition("Bearer",
                        new OpenApiSecurityScheme
                        {
                            In = ParameterLocation.Header,
                            Description = "Please enter into field the word 'Bearer' following by space and JWT",
                            Name = "Authorization",
                            Type = SecuritySchemeType.ApiKey
                        });
                    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                       {
                           {
                               new OpenApiSecurityScheme
                               {
                                   Reference = new OpenApiReference
                                     {
                                         Id = "Bearer", //The name of the previously defined security scheme.
                                         Type = ReferenceType.SecurityScheme
                                     }
                               },
                               new List<string>()
                           }
                       });
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
