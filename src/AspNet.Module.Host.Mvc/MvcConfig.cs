using System.Text.Json;
using System.Text.Json.Serialization;
using AspNet.Module.Host.Mvc.Json;
using Microsoft.Extensions.DependencyInjection;

namespace AspNet.Module.Host.Mvc;

public class MvcConfig
{
    public Action<IMvcBuilder>? Configure { get; init; }

    public bool LowercaseUrls { get; init; } = true;

    public static void ConfigureJson(IMvcBuilder mvcBuilder, JsonNamingPolicy jsonNamingPolicy) =>
        mvcBuilder.AddJsonOptions(o =>
        {
            o.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            o.JsonSerializerOptions.PropertyNamingPolicy = jsonNamingPolicy;
            o.JsonSerializerOptions.Converters.Add(new ObjectToInferredTypesConverter());
            o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(jsonNamingPolicy));
        });
}