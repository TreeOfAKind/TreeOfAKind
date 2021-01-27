using FluentValidation;

namespace TreeOfAKind.Application.Command.Trees.People.RemovePersonsFile
{
    public class RemovePersonsFileCommandValidator : AbstractValidator<RemovePersonsFileCommand>
    {
        public RemovePersonsFileCommandValidator()
        {
            RuleFor(x => x.PersonId)
                .NotEmpty();

            RuleFor(x => x.FileId)
                .NotEmpty();
        }
    }
}
