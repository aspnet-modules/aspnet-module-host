using AspNet.Module.Common;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global

namespace AspNet.Module.Host.ApiVersioning;

/// <summary>
///     Версионность АПИ Модуль
/// </summary>
public class ApiVersioningModule : IAspNetModule
{
    private readonly ApiVersioningConfig _config;

    public ApiVersioningModule() : this(null)
    {
    }

    public ApiVersioningModule(ApiVersioningConfig? config)
    {
        _config = config ?? new ApiVersioningConfig();
    }

    public void Configure(AspNetModuleContext ctx) =>
        ctx.Services.AddApiVersioning(options =>
            {
                // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
                options.ReportApiVersions = true;
                _config.VersioningConfigure?.Invoke(options);
            })
            .AddApiExplorer(options =>
            {
                // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                // note: the specified format code will format the version as "'v'major[.minor][-status]"
                options.GroupNameFormat = "'v'VVV";
                // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                // can also be used to control the format of the API version in route templates
                options.SubstituteApiVersionInUrl = true;
                _config.ExplorerConfigure?.Invoke(options);
            });
}