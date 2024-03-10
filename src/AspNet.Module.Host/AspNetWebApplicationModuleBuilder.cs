using AspNet.Module.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace AspNet.Module.Host;

public class AspNetWebApplicationModuleBuilder<T> : IAspNetWebApplicationModuleBuilder<T> where T : IAspNetModule
{
    private readonly AspNetWebApplicationBuilder _builder;

    public AspNetWebApplicationModuleBuilder(AspNetWebApplicationBuilder builder)
    {
        _builder = builder;
    }

    public WebApplication Build() => _builder.Build();

    public AspNetWebApplicationBuilder ConfigureWebHost(Action<IWebHostBuilder> configure) =>
        _builder.ConfigureWebHost(configure);

    public IAspNetWebApplicationModuleBuilder<T> DependsOn<TModule>() where TModule : IAspNetModule =>
        _builder.AddModuleDependency<T, TModule>();

    public IAspNetWebApplicationModuleBuilder<TModule> RegisterModule<TModule>(
        Func<AspNetModuleContext, TModule> moduleFactory)
        where TModule : IAspNetModule =>
        _builder.RegisterModule(moduleFactory);

    public IAspNetWebApplicationModuleBuilder<TModule> RegisterModule<TModule>(TModule module)
        where TModule : IAspNetModule => _builder.RegisterModule(module);

    public IAspNetWebApplicationModuleBuilder<TModule> RegisterModule<TModule>() where TModule : IAspNetModule, new() =>
        _builder.RegisterModule<TModule>();

    public IAspNetWebApplicationModuleBuilder<T> WithCondition(Func<IHostEnvironment, bool> condition) =>
        _builder.SetModuleCondition<T>(condition);
}