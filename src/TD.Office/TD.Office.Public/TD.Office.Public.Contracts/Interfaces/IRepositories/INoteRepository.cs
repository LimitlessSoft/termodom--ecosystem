using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Public.Contracts.Interfaces.IRepositories;
public interface INoteRepository
{
    NoteEntity GetNoteById(long id);
    bool NoteWithSameName(long userId, string name, long? excludeId = -1);
}
