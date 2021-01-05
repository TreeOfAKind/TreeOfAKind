using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.UserProfiles;

namespace TreeOfAKind.Application.DomainServices.GedcomXImport
{
    public class GedcomXToDomainTreeService : IGedcomXToDomainTreeService
    {
        private readonly IGedcomXToDomainRelationConverter _gedcomXToDomainRelationConverter;
        private readonly IGedcomXToDomainPersonConverter _gedcomXToDomainPersonConverter;

        public GedcomXToDomainTreeService(IGedcomXToDomainPersonConverter gedcomXToDomainPersonConverter,
            IGedcomXToDomainRelationConverter gedcomXToDomainRelationConverter)
        {
            _gedcomXToDomainPersonConverter = gedcomXToDomainPersonConverter;
            _gedcomXToDomainRelationConverter = gedcomXToDomainRelationConverter;
        }

        public Tree ConvertTree(UserId userId, Gx.Gedcomx gx, string treeName)
        {
            var tree = Tree.CreateNewTree(treeName, userId);

            var gxIdToPersonId = _gedcomXToDomainPersonConverter.AddPeopleToTree(gx, tree);

            _gedcomXToDomainRelationConverter.AddRelationsToTree(gx, gxIdToPersonId, tree);

            return tree;
        }
    }
}
