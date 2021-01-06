using System;
using System.Linq;
using Gx.Conclusion;
using Gx.Types;

namespace TreeOfAKind.Application.DomainServices.GedcomXImport
{
    public class GedcomXDateExtractor : IGedcomXDateExtractor
    {
        public DateTime? GetDate(Person gxPerson, FactType factType)
        {
            var fact = gxPerson?.Facts?.FirstOrDefault(f => f.KnownType == factType);

            if (DateTime.TryParse(fact?.Value, out DateTime date)) return date;

            return null;
        }
    }
}