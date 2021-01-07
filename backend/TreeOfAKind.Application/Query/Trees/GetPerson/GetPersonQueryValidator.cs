using FluentValidation;

namespace TreeOfAKind.Application.Query.Trees.GetPerson
{
    public class GetPersonQueryValidator : AbstractValidator<GetPersonQuery>
    {
        public GetPersonQueryValidator()
        {
            RuleFor(x => x.PersonId)
                .NotEmpty()
                .WithMessage("PersonId must not be empty");
        }
    }
}
