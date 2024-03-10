namespace AspNet.Module.Host.Cors.Options;

/// <summary>
///     Настройки Cors
/// </summary>
public class CorsOptions
{
    /// <summary>
    ///     Настройки Host
    /// </summary>
    public const string Name = "Cors";

    private const string AllowAnyOriginFormat = "*";

    /// <summary>
    ///     Разрешены любые адреса
    /// </summary>
    public bool AllowAnyOrigin => Origins == AllowAnyOriginFormat;

    /// <summary>
    ///     Список доступных адресов для CORS. Доступен формат *
    /// </summary>
    public string Origins { get; internal set; } = AllowAnyOriginFormat;

    /// <summary>
    ///     Получение CORS в виде списка адресов
    /// </summary>
    public string[] SplitCorsOrigins
    {
        get
        {
            if (AllowAnyOrigin)
            {
                return Array.Empty<string>();
            }

            return Origins
                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(o => RemovePostFix(o, "/")).ToArray();
        }
    }

    private static string RemovePostFix(string str, params string[] postFixes)
    {
        if (string.IsNullOrEmpty(str) || postFixes.Length == 0)
        {
            return str;
        }

        foreach (var postFix in postFixes)
        {
            if (str.EndsWith(postFix))
            {
                return str.Substring(0, str.Length - postFix.Length);
            }
        }

        return str;
    }
}