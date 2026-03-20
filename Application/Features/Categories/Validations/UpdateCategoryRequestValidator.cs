using Common.Requests.Categories;

using FluentValidation;

namespace Application.Features.Categories.Validations;

public sealed class UpdateCategoryRequestValidator : AbstractValidator<UpdateCategoryRequest>
{
    public UpdateCategoryRequestValidator()
    {
        Include(new CreateCategoryRequestValidator());

        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}
