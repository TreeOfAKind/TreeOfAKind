using System;
using TreeOfAKind.Domain.SeedWork;

namespace TreeOfAKind.Domain.Trees.People
{
    public class BirthDateMustBeBeforeDeathDateRule : IBusinessRule
    {
        public DateTime? BirthDate { get; }
        public DateTime? DeathDate { get; }
        public BirthDateMustBeBeforeDeathDateRule(DateTime? birthDate, DateTime? deathDate)
        {
            BirthDate = birthDate;
            DeathDate = deathDate;
        }

        public bool IsBroken()
            => DeathDate < BirthDate;

        public string Message => "Birth date must be before death date.";
    }
}