using System.Text.Json;
using AspNet.Module.Common;
using AspNet.Module.Host.Mvc.Responses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace AspNet.Module.Host.Mvc;

/// <summary>
///     Mvc модуль
/// </summary>
public class MvcModule : IAspNetModule
{
    private readonly MvcConfig _config;

    public MvcModule() : this(JsonNamingPolicy.CamelCase)
    {
    }

    public MvcModule(JsonNamingPolicy jsonNamingPolicy)
    {
        _config = new MvcConfig
        {
            Configure = mvcBuilder => { MvcConfig.ConfigureJson(mvcBuilder, jsonNamingPolicy); }
        };
    }

    public MvcModule(MvcConfig config)
    {
        _config = config;
    }

    public void ConfigureApp(WebApplication app) => app.MapControllers();

    public void Configure(AspNetModuleContext ctx)
    {
        ctx.Services.Configure<RouteOptions>(options => { options.LowercaseUrls = _config.LowercaseUrls; });
        var builder = ctx.Services.AddControllers();
        builder.ConfigureApiBehaviorOptions(options => options.InvalidModelStateResponseFactory =
            context => InvalidModelStateResponseFactory.ValidationErrorResult(context, ctx.Environment));
        builder.AddApplicationPart(ctx.EntryAssembly).AddControllersAsServices();
        _config.Configure?.Invoke(builder);
    }
}