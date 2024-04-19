using FluentValidation;
using GraphQL.API.Schema.Instuctors.InstructorMutations;

namespace GraphQL.API.Validators;

public class InstructorTypeInputValidator : AbstractValidator<InstructorTypeInput>
{
    public InstructorTypeInputValidator()
    {
        RuleFor(i => i.FirstName).NotEmpty();
        RuleFor(i => i.LastName).NotEmpty();
    }
}