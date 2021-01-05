using System;
using System.Collections.Generic;
using System.Linq;
using Gx.Types;
using TreeOfAKind.Application.Command.Trees.TreeAdministration.CreateTreeFromFile;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;
using Xunit;
using AutoFixture;
using NSubstitute;
using TreeOfAKind.Application.DomainServices.GedcomXImport;
using TreeOfAKind.Domain.UserProfiles;


namespace TreeOfAKind.UnitTests.Trees
{
    public class FileImport
    {
        private readonly Fixture _fixture = new Fixture();

        [Theory]
        [InlineDataAttribute(Gender.Female, RelationType.Mother)]
        [InlineDataAttribute(Gender.Male, RelationType.Father)]
        [InlineDataAttribute(Gender.Unknown, RelationType.Father)]
        [InlineDataAttribute(Gender.Other, RelationType.Father)]
        public void ConvertRelationType_ParentChildRelation_RelationSpecifiedBasedOnGender(Gender gender, RelationType expectedRelationType)
        {
            var converter = new GedcomXToDomainRelationTypeConverter();
            var person = Person.CreateNewPerson(null!, "Name", "LastName", gender,
                DateTime.Parse("10.10.1998"), null, "", "");
            var people = new List<Person>{person};
            var relationType = converter.ConvertRelationType(RelationshipType.ParentChild, people, person.Id);

            Assert.Equal(expectedRelationType, relationType);
        }

        [Theory]
        [InlineDataAttribute(Gender.Female, RelationType.Father)]
        [InlineDataAttribute(Gender.Male, RelationType.Father)]
        [InlineDataAttribute(Gender.Unknown, RelationType.Father)]
        [InlineDataAttribute(Gender.Other, RelationType.Father)]
        public void ConvertRelationType_ParentChildRelation_CannotSpecifyRelationOnGender(Gender gender, RelationType expectedRelationType)
        {
            var converter = new GedcomXToDomainRelationTypeConverter();
            var person = Person.CreateNewPerson(null!, "Name", "LastName", gender,
                DateTime.Parse("10.10.1998"), null, "", "");
            var people = new List<Person>{person};
            var relationType = converter.ConvertRelationType(RelationshipType.ParentChild, people, new PersonId(Guid.NewGuid()));

            Assert.Equal(expectedRelationType, relationType);
        }

        [Theory]
        [InlineDataAttribute(Gender.Female)]
        [InlineDataAttribute(Gender.Male)]
        [InlineDataAttribute(Gender.Unknown)]
        [InlineDataAttribute(Gender.Other)]
        public void ConvertRelationType_CoupleRelation_RelationIsConvertedToSpouse(Gender gender)
        {
            var converter = new GedcomXToDomainRelationTypeConverter();
            var person = Person.CreateNewPerson(null!, "Name", "LastName", gender,
                DateTime.Parse("10.10.1998"), null, "", "");
            var people = new List<Person>{person};
            var relationType = converter.ConvertRelationType(RelationshipType.Couple, people, person.Id);

            Assert.Equal(RelationType.Spouse, relationType);
        }

        [Theory]
        [InlineDataAttribute(RelationshipType.NULL)]
        [InlineDataAttribute(RelationshipType.OTHER)]
        public void ConvertRelationType_UnspecifiedRelation_RelationIsConvertedToUnknown(RelationshipType relationshipType)
        {
            var converter = new GedcomXToDomainRelationTypeConverter();
            var person = Person.CreateNewPerson(null!, "Name", "LastName", Gender.Female,
                DateTime.Parse("10.10.1998"), null, "", "");
            var people = new List<Person>{person};
            var relationType = converter.ConvertRelationType(relationshipType, people, person.Id);

            Assert.Equal(RelationType.Unknown, relationType);
        }

        [Theory]
        [InlineDataAttribute(GenderType.Female, Gender.Female)]
        [InlineDataAttribute(GenderType.Male, Gender.Male)]
        [InlineDataAttribute(GenderType.Unknown, Gender.Unknown)]
        [InlineDataAttribute(GenderType.OTHER, Gender.Other)]
        [InlineDataAttribute(GenderType.NULL, Gender.Unknown)]
        public void ConvertGender_GenderTypeProvided_ProperGender(GenderType gender, Gender expected)
        {
            var converter = new GedcomXToDomainGenderConverter();
            var convertedGender = converter.ConvertGender(gender);

            Assert.Equal(expected, convertedGender);
        }

        [Fact]
        public void AddPeopleToTree_OnePersonInGedcomXFile_ConvertersAreCalledPersonInTree()
        {
            var gx = new Gx.Gedcomx();
            var person = new Gx.Conclusion.Person().SetGender(GenderType.Male).SetName("ASDDF");
            gx.AddPerson(person);
            var tree = _fixture.Create<Tree>();

            var genderConverter = Substitute.For<IGedcomXToDomainGenderConverter>();
            var nameExtractor = Substitute.For<IGedcomXNameExtractor>();
            nameExtractor.ExtractName(Arg.Any<Gx.Conclusion.Person>(), NamePartType.Given).Returns("Bati");
            nameExtractor.ExtractName(Arg.Any<Gx.Conclusion.Person>(), NamePartType.Surname).Returns("Chro");
            var dateExtractor= Substitute.For<IGedcomXDateExtractor>();
            var noteExtractor = Substitute.For<IGedcomXNoteExtractor>();
            var converter = new GedcomXToDomainPersonConverter(
                genderConverter, nameExtractor, dateExtractor, noteExtractor);

            _ = converter.AddPeopleToTree(gx, tree);

            genderConverter.ReceivedWithAnyArgs().ConvertGender(default);
            nameExtractor.ReceivedWithAnyArgs().ExtractName(default,default);
            nameExtractor.DidNotReceiveWithAnyArgs().ExtractFullName(default);


            Assert.Single(tree.People);
            var personInTree = tree.People.FirstOrDefault();

            Assert.NotNull(personInTree);
            Assert.Equal("Bati", personInTree.Name);
            Assert.Equal("Chro", personInTree.LastName);
        }

        [Fact]
        public void ConvertTree_ValidData_InterfacesAreCalled()
        {
            var gedcomXToDomainRelationConverter = Substitute.For<IGedcomXToDomainRelationConverter>();
            var gedcomXToDomainPersonConverter = Substitute.For<IGedcomXToDomainPersonConverter>();
            var converter = new GedcomXToDomainTreeService(gedcomXToDomainPersonConverter,gedcomXToDomainRelationConverter);

            _ = converter.ConvertTree(new UserId(Guid.NewGuid()), new Gx.Gedcomx(), "TreeName");

            gedcomXToDomainPersonConverter.ReceivedWithAnyArgs().AddPeopleToTree(default, default);
            gedcomXToDomainRelationConverter.ReceivedWithAnyArgs().AddRelations(default, default, default);
        }

    }
}
