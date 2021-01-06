using System;
using System.Collections.Generic;
using System.Linq;
using Gx.Types;
using TreeOfAKind.Application.Command.Trees.TreeAdministration.CreateTreeFromFile;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;
using Xunit;
using AutoFixture;
using Gx.Conclusion;
using NSubstitute;
using NSubstitute.Extensions;
using NSubstitute.ReceivedExtensions;
using TreeOfAKind.Application.DomainServices.GedcomXImport;
using TreeOfAKind.Domain.UserProfiles;
using Gender = TreeOfAKind.Domain.Trees.People.Gender;
using Person = TreeOfAKind.Domain.Trees.People.Person;


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
            var converter = new GedcomXToDomainTreeConverter(gedcomXToDomainPersonConverter,gedcomXToDomainRelationConverter);

            _ = converter.ConvertTree(new UserId(Guid.NewGuid()), new Gx.Gedcomx(), "TreeName");

            gedcomXToDomainPersonConverter.ReceivedWithAnyArgs().AddPeopleToTree(default, default);
            gedcomXToDomainRelationConverter.ReceivedWithAnyArgs().AddRelationsToTree(default, default, default);
        }

        [Fact]
        public void AddRelationsToTree_OneRelation_RelationIsAddedToTree()
        {
            var gedcomXToDomainRelationTypeConverter = Substitute.For<IGedcomXToDomainRelationTypeConverter>();
            gedcomXToDomainRelationTypeConverter.ConvertRelationType(default, default, default)
                .ReturnsForAnyArgs(RelationType.Mother);
            var converter = new GedcomXToDomainRelationConverter(gedcomXToDomainRelationTypeConverter);

            var tree = Tree.CreateNewTree("Drzewko", new UserId(Guid.NewGuid()));

            var p1 = tree.AddPerson("A", "AA", Gender.Female, null, null, null, null);
            var p2 = tree.AddPerson("B", "BB", Gender.Male, null, null, null, null);

            var gxp1 = (Gx.Conclusion.Person)new Gx.Conclusion.Person().SetId(p1.Id.Value.ToString());
            var gxp2 = (Gx.Conclusion.Person)new Gx.Conclusion.Person().SetId(p2.Id.Value.ToString());
            var gx = new Gx.Gedcomx().SetRelationship(new Relationship().SetPerson1(gxp1).SetPerson2(gxp2)
                .SetType(RelationshipType.ParentChild));

            var dict = new Dictionary<string, PersonId>
            {
                {gxp1.Id, p1.Id},
                {gxp2.Id, p2.Id},
            };

            converter.AddRelationsToTree(gx, dict, tree);

            gedcomXToDomainRelationTypeConverter.ReceivedWithAnyArgs().ConvertRelationType(default, default, default);
            Assert.Single(tree.TreeRelations.Relations);
            Assert.Equal(RelationType.Mother, tree.TreeRelations.Relations.FirstOrDefault()?.RelationType);
        }

    }
}
