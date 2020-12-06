using System;
using TreeOfAKind.Domain.SeedWork;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;
using Xunit;

namespace TreeOfAKind.UnitTests.Trees
{
    public class TreeRelationsDomain
    {
        private static PersonId PersonId1
            => new PersonId(new Guid("24711044-8202-4893-829b-e8391d95bae9"));

        private static PersonId PersonId2
            => new PersonId(new Guid("f8f55736-2b02-428c-8e0d-3bf3ce464555"));

        private static PersonId PersonId3
            => new PersonId(new Guid("903caa63-86f8-46ac-9428-622244bbfe91"));

        private TreeRelations TreeRelations { get; }

        public TreeRelationsDomain()
        {
            TreeRelations = new TreeRelations();
        }

        [Theory]
        [InlineData(RelationType.Father, RelationType.Father, RelationType.Father)]
        [InlineData(RelationType.Mother, RelationType.Mother, RelationType.Mother)]
        [InlineData(RelationType.Parent, RelationType.Parent, RelationType.Parent)]
        [InlineData(RelationType.Mother, RelationType.Father, RelationType.Parent)]
        public void AddRelation_AddCycle_ThrowsException(RelationType first, RelationType second, RelationType third)
        {
            TreeRelations.AddRelation(PersonId1,PersonId2, first);
            TreeRelations.AddRelation(PersonId2,PersonId3, second);

            Assert.Throws<BusinessRuleValidationException>(() =>
                TreeRelations.AddRelation(PersonId3,PersonId1, third));
        }

        [Fact]
        public void AddRelation_AddCycleOfTwo_ThrowsException()
        {
            TreeRelations.AddRelation(PersonId1,PersonId2, RelationType.Mother);

            Assert.Throws<BusinessRuleValidationException>(() =>
                TreeRelations.AddRelation(PersonId2,PersonId1, RelationType.Father));
        }

        [Fact]
        public void AddRelation_ValidData_AddsRelations()
        {
            TreeRelations.AddRelation(PersonId1,PersonId2, RelationType.Father);
            TreeRelations.AddRelation(PersonId2,PersonId3, RelationType.Father);
            TreeRelations.AddRelation(PersonId3,PersonId1, RelationType.Spouse);

            Assert.Contains(new Relation(PersonId3, PersonId1, RelationType.Spouse), TreeRelations.Relations);
            Assert.Contains(new Relation(PersonId1, PersonId3, RelationType.Spouse), TreeRelations.Relations);
            Assert.Equal(4, TreeRelations.Relations.Count);
        }

        [Fact]
        public void AddRelation_RelationToOneself_ThrowsException()
        {
            Assert.Throws<BusinessRuleValidationException>(() =>
                TreeRelations.AddRelation(PersonId1, PersonId1, RelationType.Father));
        }

        [Fact]
        public void RemoveRelation_ValidData_RemovesSymmetric()
        {
            TreeRelations.AddRelation(PersonId1,PersonId2, RelationType.Father);
            TreeRelations.AddRelation(PersonId2,PersonId3, RelationType.Father);
            TreeRelations.AddRelation(PersonId3,PersonId1, RelationType.Spouse);

            TreeRelations.RemoveRelation(PersonId1, PersonId3);
            Assert.DoesNotContain(new Relation(PersonId3, PersonId1, RelationType.Spouse), TreeRelations.Relations);
            Assert.DoesNotContain(new Relation(PersonId1, PersonId3, RelationType.Spouse), TreeRelations.Relations);
            Assert.Equal(2, TreeRelations.Relations.Count);
        }
    }
}
