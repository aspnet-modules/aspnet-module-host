using Asp.Versioning;
using Asp.Versioning.ApiExplorer;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace AspNet.Module.Host.ApiVersioning;

public class ApiVersioningConfig
{
    public Action<ApiExplorerOptions>? ExplorerConfigure { get; init; }
    public Action<ApiVersioningOptions>? VersioningConfigure { get; init; }
}