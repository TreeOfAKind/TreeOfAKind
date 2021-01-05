using System.Collections.Generic;
using Gx.Common;

namespace TreeOfAKind.Application.DomainServices.GedcomXImport
{
    public interface IGedcomXNoteExtractor
    {
        string GetNote(IEnumerable<Note> notes, string subject);
    }
}