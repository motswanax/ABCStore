using Common.Pipelines;
using Common.Wrappers;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Filters;

public sealed class ValidateFluentValidationFilter(IServiceProvider serviceProvider) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        foreach (var arg in context.ActionArguments.Values)
        {
            if (arg is null || arg is not IValidateMe)
                continue;

            var validatorType = typeof(IValidator<>).MakeGenericType(arg.GetType());
            var validator = serviceProvider.GetService(validatorType);

            if (validator is null)
                continue;

            var argType = arg.GetType();
            var validationContextType = typeof(ValidationContext<>).MakeGenericType(argType);
            var validationContext = Activator.CreateInstance(validationContextType, arg);

            var validateAsync = validatorType.GetMethod("ValidateAsync", [validationContextType, typeof(CancellationToken)]);
            if (validateAsync is null)
                continue;

            var validateTask = (Task)validateAsync.Invoke(validator, [validationContext!, context.HttpContext.RequestAborted])!;
            await validateTask.ConfigureAwait(false);

            var resultProperty = validateTask.GetType().GetProperty("Result");
            var validationResult = resultProperty?.GetValue(validateTask);

            var isValid = (bool?)validationResult?.GetType().GetProperty("IsValid")?.GetValue(validationResult) ?? true;
            if (isValid)
                continue;

            var errorsObj = validationResult?.GetType().GetProperty("Errors")?.GetValue(validationResult) as IEnumerable<object>;
            var messages = errorsObj?
                .Select(e => e.GetType().GetProperty("ErrorMessage")?.GetValue(e)?.ToString())
                .Where(m => !string.IsNullOrWhiteSpace(m))
                .Cast<string>()
                .ToList() ?? [];

            context.Result = new BadRequestObjectResult(ResponseWrapper.Fail(messages));
            return;
        }

        await next();
    }
}
