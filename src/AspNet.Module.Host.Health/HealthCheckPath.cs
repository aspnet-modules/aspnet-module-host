namespace AspNet.Module.Host.Health;

/// <summary>
///     Эндпоинты живучести сериса
/// </summary>
public static class HealthCheckPath
{
    /// <summary>
    ///     Проверка ready
    /// </summary>
    public const string ReadyPath = "/ready";

    /// <summary>
    ///     Проверка self
    /// </summary>
    public const string SelfPath = "/self";
}