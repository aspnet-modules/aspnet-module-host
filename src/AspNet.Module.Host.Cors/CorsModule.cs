using AspNet.Module.Common;
using AspNet.Module.Host.Cors.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspNet.Module.Host.Cors;

/// <summary>
///     CORS Модуль
/// </summary>
public class CorsModule : IAspNetModule
{
    /// <summary>
    ///     Название Cors
    /// </summary>
    public const string DefaultCorsPolicyName = "App";

    public void ConfigureApp(WebApplication app) => app.UseCors(DefaultCorsPolicyName);

    /// <inheritdoc />
    public virtual void Configure(AspNetModuleContext context)
    {
        var corsOptions = new CorsOptions();
        context.Configuration.GetSection(CorsOptions.Name)
            .Bind(corsOptions, o => { o.BindNonPublicProperties = true; });
        context.Services.AddCors(options =>
        {
            options.AddPolicy(DefaultCorsPolicyName, pBuilder =>
            {
                if (corsOptions.AllowAnyOrigin)
                {
                    pBuilder.AllowAnyOrigin();
                }
                else
                {
                    pBuilder.WithOrigins(corsOptions.SplitCorsOrigins);
                }

                pBuilder.AllowAnyHeader().AllowAnyMethod();
            });
        });
    }
}