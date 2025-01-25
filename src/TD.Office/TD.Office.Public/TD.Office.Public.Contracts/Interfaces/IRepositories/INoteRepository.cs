using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Public.Contracts.Interfaces.IRepositories;

public interface INoteRepository
{
    NoteEntity GetById(long id);
    bool HasMoreThanOne(long userId);
    bool Exists(long userId, string name, long? excludeId = null);

    /// <summary>
    /// Returns note id and name for the given user.
    /// </summary>
    /// <param name="contextUserId"></param>
    /// <returns></returns>
    Dictionary<long, string> GetNotesIdentifiers(long contextUserId);
    void HardDelete(long id);
    void Update(NoteEntity entity);
    void UpdateOrCreate(NoteEntity entity);
}
