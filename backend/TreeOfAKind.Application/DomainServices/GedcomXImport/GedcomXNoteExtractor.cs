using System.Collections.Generic;
using System.Linq;
using Gx.Common;

namespace TreeOfAKind.Application.DomainServices.GedcomXImport
{
    public class GedcomXNoteExtractor : IGedcomXNoteExtractor
    {
        public string GetNote(IEnumerable<Note> notes, string subject)
            => notes?.FirstOrDefault(n => n.Subject == subject)?.Text ?? "";
    }
}