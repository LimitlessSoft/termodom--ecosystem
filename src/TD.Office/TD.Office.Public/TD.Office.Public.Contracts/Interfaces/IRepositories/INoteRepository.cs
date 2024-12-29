using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Public.Contracts.Interfaces.IRepositories;
public interface INoteRepository
{
    NoteEntity GetById(long id);
    bool Exists(long userId, string name, long? excludeId = -1);
}
