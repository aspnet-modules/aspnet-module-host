using AspNet.Module.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global

namespace AspNet.Module.Host;

public class AspNetWebApplicationBuilder
{
    private readonly WebApplicationBuilder _builder;
    private readonly Dictionary<Type, Func<IHostEnvironment, bool>> _moduleConditions = new();
    private readonly Dictionary<Type, List<Type>> _moduleDependencies = new();
    private readonly Dictionary<Type, Func<AspNetModuleContext, IAspNetModule>> _moduleFactories = new();


    internal AspNetWebApplicationBuilder(WebApplicationBuilder builder, AspNetModuleContext context)
    {
        _builder = builder;
        Context = context;
    }

    /// <summary>
    ///     Конфигурация <see cref="WebApplication" />
    /// </summary>
    public Action<WebApplication>? ConfigureApp { get; set; } = app =>
    {
        if (app.Environment.IsProduction())
        {
            app.UseHsts();
            app.UseHttpsRedirection();
        }
    };

    /// <summary>
    ///     Контекст
    /// </summary>
    public AspNetModuleContext Context { get; }

    /// <summary>
    ///     Хост
    /// </summary>
    public IHostBuilder Host => _builder.Host;

    public IAspNetWebApplicationModuleBuilder<TModule> AddModuleDependency<TModule, TModuleDependency>()
        where TModule : IAspNetModule where TModuleDependency : IAspNetModule
    {
        _moduleDependencies[typeof(TModule)].Add(typeof(TModuleDependency));
        return new AspNetWebApplicationModuleBuilder<TModule>(this);
    }

    public WebApplication Build()
    {
        var modules = new List<IAspNetModule>();

        var sortedModules = ModuleDependenciesSorter.TopologicalSort(_moduleDependencies);

        if (sortedModules.Count != _moduleFactories.Count)
        {
            var errorMessage = string.Join(",",
                _moduleFactories.Keys.Where(type => !sortedModules.Contains(type)).Select(type => type.Name));
            throw new ArgumentException(
                $"Невозможно сформировать последовательность конфигурации модулей: {errorMessage}");
        }

        foreach (var moduleType in sortedModules)
        {
            if (!_moduleFactories.TryGetValue(moduleType, out var moduleFactory))
            {
                throw new ArgumentException($"Невозможно проинициализировать модуль {moduleType.Name}");
            }

            if (_moduleConditions.TryGetValue(moduleType, out var condition) &&
                !condition(Context.Environment))
            {
                continue;
            }

            var module = moduleFactory.Invoke(Context);
            module.Configure(Context);
            modules.Add(module);
        }

        var app = _builder.Build();
        ConfigureApp?.Invoke(app);
        foreach (var module in modules)
        {
            module.ConfigureApp(app);
        }

        return app;
    }

    public AspNetWebApplicationBuilder ConfigureWebHost(Action<IWebHostBuilder> configure)
    {
        configure(_builder.WebHost);
        return this;
    }

    public AspNetWebApplicationModuleBuilder<TModule> RegisterModule<TModule>(TModule module)
        where TModule : IAspNetModule
    {
        var moduleType = module.GetType();
        _moduleFactories[moduleType] = _ => module;
        _moduleDependencies[moduleType] = new List<Type>();
        return new AspNetWebApplicationModuleBuilder<TModule>(this);
    }

    public IAspNetWebApplicationModuleBuilder<TModule> RegisterModule<TModule>(
        Func<AspNetModuleContext, TModule> moduleFactory)
        where TModule : IAspNetModule
    {
        _moduleFactories[typeof(TModule)] = context => moduleFactory(context);
        _moduleDependencies[typeof(TModule)] = new List<Type>();
        return new AspNetWebApplicationModuleBuilder<TModule>(this);
    }

    public IAspNetWebApplicationModuleBuilder<TModule> RegisterModule<TModule>() where TModule : IAspNetModule, new()
    {
        _moduleFactories[typeof(TModule)] = _ => new TModule();
        _moduleDependencies[typeof(TModule)] = new List<Type>();
        return new AspNetWebApplicationModuleBuilder<TModule>(this);
    }

    public AspNetWebApplicationModuleBuilder<TModule> SetModuleCondition<TModule>(
        Func<IHostEnvironment, bool> condition)
        where TModule : IAspNetModule
    {
        _moduleConditions[typeof(TModule)] = condition;
        return new AspNetWebApplicationModuleBuilder<TModule>(this);
    }
}