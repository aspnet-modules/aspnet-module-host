using System.Reflection;
using AspNet.Module.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AspNet.Module.Host;

public class AspNetWebApplication
{
    public static AspNetWebApplicationBuilder CreateBuilder(Assembly entryAssembly, params string[] args)
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            Args = args
        });

        var context = new AspNetModuleContext
        {
            Configuration = builder.Configuration,
            Environment = builder.Environment,
            Services = builder.Services,
            EntryAssembly = entryAssembly
        };

        // Configuration
        builder.Configuration
            .SetBasePath(builder.Environment.ContentRootPath)
            .AddJsonFile("appsettings.json", true, true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
            .AddEnvironmentVariables();
        if (builder.Environment.IsDevelopment())
        {
            builder.Configuration.AddUserSecrets(context.EntryAssembly);
#if DEBUG
            if (args.Any(x => x.Contains("--local")))
            {
                builder.Configuration.AddJsonFile("appsettings.Local.json", true);
            }
#endif
        }

        // Port
        var port = Environment.GetEnvironmentVariable("PORT");
        if (!string.IsNullOrWhiteSpace(port))
        {
            builder.WebHost.UseUrls("http://*:" + port);
        }

        // Kestrel
        var kestrelOptions = builder.Configuration.GetSection("Kestrel");
        if (kestrelOptions != null && kestrelOptions.Exists())
        {
            builder.Services.Configure<KestrelServerOptions>(kestrelOptions);
        }

        return new AspNetWebApplicationBuilder(builder, context);
    }

    public static AspNetWebApplicationBuilder CreateBuilder(params string[] args) =>
        CreateBuilder(Assembly.GetEntryAssembly()!, args);
}