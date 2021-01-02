using System.Collections.Generic;
using System.IO;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;
using Person = Gx.Conclusion.Person;

namespace TreeOfAKind.Application.DomainServices
{
    public class FamilyTreeFileExporter : IFamilyTreeFileExporter
    {
        private readonly IGedcomToXmlStreamConverter _converter;

        public FamilyTreeFileExporter(IGedcomToXmlStreamConverter converter)
        {
            _converter = converter;
        }

        public Stream Export(Tree tree)
        {
            var idPersonDict = CreatePeople(tree);

            var gx = new Gx.Gedcomx();

            foreach (var (_, person) in idPersonDict)
            {
                gx.AddPerson(person);
            }

            foreach (var relation in tree.TreeRelations.Relations)
            {
                var from = idPersonDict[relation.From];
                var to = idPersonDict[relation.To];

                gx.AddRelation(relation, from, to);
            }

            return _converter.ToXmlStream(gx);
        }

        private static Dictionary<PersonId, Person> CreatePeople(Tree tree)
        {
            var idPersonDict = new Dictionary<PersonId, Person>();

            foreach (var person in tree.People)
            {
                var personModel = (Person) new Person()
                    .AddNameLastname(person)
                    .AddDeathDate(person)
                    .AddBirthDate(person)
                    .AddBiography(person)
                    .AddDescription(person)
                    .AddFiles(person)
                    .SetId(person.Id.Value.ToString());

                idPersonDict.Add(person.Id, personModel);
            }

            return idPersonDict;
        }
    }
}
