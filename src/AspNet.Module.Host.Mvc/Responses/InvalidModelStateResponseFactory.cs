using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Hosting;

namespace AspNet.Module.Host.Mvc.Responses;

internal static class InvalidModelStateResponseFactory
{
    private const string DefaultMessage = "Найдены ошибки при получении данных.";

    public static IActionResult ValidationErrorResult(ActionContext context, IHostEnvironment hostEnvironment)
    {
        if (context.ModelState.ValidationState != ModelValidationState.Invalid)
        {
            return new BadRequestObjectResult(context.ModelState);
        }

        var modelErrorsQuery = context.ModelState
            .Where(x => x.Value?.ValidationState == ModelValidationState.Invalid)
            .Where(x => !string.IsNullOrEmpty(x.Key));

        if (hostEnvironment.IsProduction())
            // https://jira.domrf.ru/browse/TIM-1261 - 3 скрин
        {
            modelErrorsQuery = modelErrorsQuery.Where(x => !x.Key.Contains("$."));
        }

        var detailErrors = modelErrorsQuery
            .ToDictionary(x => x.Key,
                x => (object)GetModelErrorMessage(x.Value));

        var errorResult = new
        {
            Code = "ValidationError",
            Message = DefaultMessage,
            Details = new Dictionary<string, object>
            {
                { "validation", detailErrors }
            }
        };

        return new BadRequestObjectResult(errorResult);
    }

    private static string GetModelErrorMessage(ModelStateEntry? modelStateEntry) =>
        modelStateEntry == null
            ? string.Empty
            : string.Join(", ", modelStateEntry.Errors.Select(e => e.ErrorMessage));
}