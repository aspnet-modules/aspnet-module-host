using AspNet.Module.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace AspNet.Module.Host;

public interface IAspNetWebApplicationModuleBuilder<T> where T : IAspNetModule
{
    WebApplication Build();
    AspNetWebApplicationBuilder ConfigureWebHost(Action<IWebHostBuilder> configure);
    IAspNetWebApplicationModuleBuilder<T> DependsOn<TModule>() where TModule : IAspNetModule;
    IAspNetWebApplicationModuleBuilder<TModule> RegisterModule<TModule>(TModule module) where TModule : IAspNetModule;

    IAspNetWebApplicationModuleBuilder<TModule> RegisterModule<TModule>(
        Func<AspNetModuleContext, TModule> moduleFactory)
        where TModule : IAspNetModule;

    IAspNetWebApplicationModuleBuilder<TModule> RegisterModule<TModule>() where TModule : IAspNetModule, new();
    IAspNetWebApplicationModuleBuilder<T> WithCondition(Func<IHostEnvironment, bool> condition);
}