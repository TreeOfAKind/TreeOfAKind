using System;
using Gx.Conclusion;
using Gx.Types;

namespace TreeOfAKind.Application.DomainServices.GedcomXImport
{
    public interface IGedcomXDateExtractor
    {
        public DateTime? GetDate(Person gxPerson, FactType factType);
    }
}