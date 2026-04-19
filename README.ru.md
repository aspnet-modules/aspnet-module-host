# Модули Host

## Версионность API - ApiVersioning

Модуль добавляет версионность АПИ.

```sh
dotnet add package AspNet.Module.Host.ApiVersioning
```

```cs
using AspNet.Module.Host.ApiVersioning;

var builder = AspNetWebApplication.CreateBuilder(args);
builder.RegisterModule<ApiVersioningModule>();
```

## CORS

Модуль политики CORS.

```sh
dotnet add package AspNet.Module.Host.Cors
```

```cs
using AspNet.Module.Host.Cors;

var builder = AspNetWebApplication.CreateBuilder(args);
builder.RegisterModule<CorsModule>();
```

## Работоспособность Liveness/Readiness - Health

Модуль добавляет эндпоинты
для [Liveness/Readiness](https://kubernetes.io/ru/docs/tasks/configure-pod-container/configure-liveness-readiness-startup-probes/).

```sh
dotnet add package AspNet.Module.Host.Health
```

```cs
using AspNet.Module.Host.Health;

var builder = AspNetWebApplication.CreateBuilder(args);
builder.RegisterModule<HealthModule>();
```

## Контроллеры - Mvc

Модуль регистрации контроллеров.

```sh
dotnet add package AspNet.Module.Host.Mvc
```

```cs
using AspNet.Module.Host.Mvc;

var builder = AspNetWebApplication.CreateBuilder(args);
builder.RegisterModule<MvcModule>();
```
