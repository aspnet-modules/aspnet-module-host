using AspNet.Module.Host;
using AspNet.Module.Host.ApiVersioning;
using AspNet.Module.Host.Mvc;

var builder = AspNetWebApplication.CreateBuilder(args);
builder.RegisterModule<MvcModule>();
builder.RegisterModule<ApiVersioningModule>();
// builder.RegisterModule<SwaggerApiVersioningModule>();

var app = builder.Build();

await app.RunAsync();