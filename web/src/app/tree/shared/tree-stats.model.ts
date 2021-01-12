export interface TreeStats {
  totalNumberOfPeople: number,
  averageLifespanInDays: number,
  numberOfPeopleOfEachGender: {
    Female: number,
    Male: number,
    Other: number,
    Unknown: number
  },
  numberOfLivingPeople: number,
  numberOfDeceasedPeople: number,
  averageNumberOfChildren: number,
  numberOfMarriedPeople: number,
  numberOfSinglePeople: number
}
