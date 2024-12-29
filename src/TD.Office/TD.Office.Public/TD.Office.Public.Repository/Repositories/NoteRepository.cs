using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Interfaces.IRepositories;
using LSCore.Contracts.Exceptions;

namespace TD.Office.Public.Repository.Repositories;
public class NoteRepository(OfficeDbContext dbContext) : INoteRepository
{
    public NoteEntity GetNoteById(long id)
    {
        var entity = dbContext.Notes.Where(x => x.Id == id && x.IsActive).FirstOrDefault();

        if (entity == null)
            throw new LSCoreNotFoundException();

        return entity;
    }

    /// <summary>
    /// Determines whether a note with the specified name already exists for the given user.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool NoteWithSameName(long userId, string name, long? excludeId = -1) =>
        dbContext.Notes.Any(x => x.IsActive && x.Name.Equals(name) && x.CreatedBy == userId && x.Id != excludeId);
}
