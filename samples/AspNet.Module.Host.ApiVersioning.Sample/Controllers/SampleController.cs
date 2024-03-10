using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace AspNet.Module.Host.ApiVersioning.Sample.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class SampleController : ControllerBase
{
    /// <summary>
    ///     V1
    /// </summary>
    [HttpGet]
    [Route("v1")]
    public Task<string> V1(CancellationToken ct) => Task.FromResult("V1");

    /// <summary>
    ///     V2
    /// </summary>
    [ApiVersion("2.0")]
    [HttpGet]
    [Route("v2")]
    public Task<string> V2(CancellationToken ct) => Task.FromResult("V2");
}