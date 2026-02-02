using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using Scalar.AspNetCore;

namespace BaseApp.Web.Code.Infrastructure.Api.OpenApi
{
    public static class OpenApiExtensions
    {
        public static void AddAppWebOpenApi(this IServiceCollection services)
        {
            services.AddOpenApi(ApiConstants.SiteGroupName, options =>
            {
                options.AddDocumentTransformer(new AppOpenApiDocumentTransformer("Site API"));
            });
            services.AddOpenApi(ApiConstants.AdminGroupName, options =>
            {
                options.AddDocumentTransformer(new AppOpenApiDocumentTransformer("Admin API"));
            });
        }

        public static void UseAppWebOpenApi(this IEndpointRouteBuilder endpoints, bool isDev)
        {
            endpoints.MapOpenApi();

            endpoints.MapScalarApiReference(options =>
            {
                options
                    .WithTitle("API Documentation")
                    .WithTheme(ScalarTheme.Purple)
                    .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
                    .WithEndpointPrefix("/scalar/{documentName}")
                    .WithOpenApiRoutePattern("/openapi/{documentName}.json");
                
                if (isDev)
                {
                    var configuration = endpoints.ServiceProvider.GetRequiredService<IConfiguration>();
                    var bearerToken = configuration["OpenApiScalarBearerToken"];
                    if (!string.IsNullOrWhiteSpace(bearerToken))
                    {
                        options
                            .WithPreferredScheme("Bearer")
                            .WithHttpBearerAuthentication(auth =>
                            {
                                auth.Token = bearerToken;
                            });
                    }
                }
            });
        }
    }
    
    internal sealed class AppOpenApiDocumentTransformer(string title) : IOpenApiDocumentTransformer
    {
        public System.Threading.Tasks.Task TransformAsync(
            OpenApiDocument document,
            OpenApiDocumentTransformerContext context,
            System.Threading.CancellationToken cancellationToken)
        {
            document.Info = new OpenApiInfo { Title = title, Version = "v1" };
            
            document.Components ??= new OpenApiComponents();
            document.Components.SecuritySchemes ??= new System.Collections.Generic.Dictionary<string, IOpenApiSecurityScheme>();

            document.Components.SecuritySchemes.Add("Bearer", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter JWT Bearer token"
            });
            
            document.Security =
            [
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecuritySchemeReference("Bearer", document),
                        new System.Collections.Generic.List<string>()
                    }
                }
            ];
            
            document.SetReferenceHostDocument();
            return System.Threading.Tasks.Task.CompletedTask;
        }
    }
}
