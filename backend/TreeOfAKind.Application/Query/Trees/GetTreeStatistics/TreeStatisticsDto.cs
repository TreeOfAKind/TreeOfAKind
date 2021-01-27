using System;
using System.Collections.Generic;
using TreeOfAKind.Domain.Trees.People;

namespace TreeOfAKind.Application.Query.Trees.GetTreeStatistics
{
    public record TreeStatisticsDto
    {
        public int TotalNumberOfPeople { get; }
        public double AverageLifespanInDays { get; }
        public Dictionary<Gender, int> NumberOfPeopleOfEachGender { get; }
        public int NumberOfLivingPeople { get; }
        public int NumberOfDeceasedPeople { get; }
        public double AverageNumberOfChildren { get; }
        public int NumberOfMarriedPeople { get; }
        public int NumberOfSinglePeople { get; }

        public TreeStatisticsDto(int totalNumberOfPeople, double averageLifespanInDays,
            Dictionary<Gender, int> numberOfPeopleOfEachGender, int numberOfLivingPeople, int numberOfDeceasedPeople,
            double averageNumberOfChildren, int numberOfMarriedPeople, int numberOfSinglePeople)
        {
            TotalNumberOfPeople = totalNumberOfPeople;
            AverageLifespanInDays = averageLifespanInDays;

            NumberOfPeopleOfEachGender = new Dictionary<Gender, int>();
            foreach (var gender in (Gender[])Enum.GetValues(typeof(Gender)))
            {
                var count = 0;
                numberOfPeopleOfEachGender?.TryGetValue(gender, out count);
                NumberOfPeopleOfEachGender.Add(gender, count);
            }

            NumberOfLivingPeople = numberOfLivingPeople;
            NumberOfDeceasedPeople = numberOfDeceasedPeople;
            AverageNumberOfChildren = averageNumberOfChildren;
            NumberOfMarriedPeople = numberOfMarriedPeople;
            NumberOfSinglePeople = numberOfSinglePeople;
        }
    }
}
