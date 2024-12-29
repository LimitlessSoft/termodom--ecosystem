using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Interfaces.IRepositories;
using LSCore.Contracts.Exceptions;

namespace TD.Office.Public.Repository.Repositories;
public class NoteRepository(OfficeDbContext dbContext) : INoteRepository
{
    public NoteEntity GetById(long id)
    {
        var entity = dbContext.Notes.FirstOrDefault(x => x.Id == id && x.IsActive);

        if (entity == null)
            throw new LSCoreNotFoundException();

        return entity;
    }

    public bool HasMoreThanOne(long userId) =>
        dbContext.Notes.Count(x => x.CreatedBy == userId && x.IsActive) > 1;

    /// <summary>
    /// Determines whether a note with the specified name already exists for the given user.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="name"></param>
    /// <param name="excludeId">If passed, it will exclude id in search</param>
    /// <returns></returns>
    public bool Exists(long userId, string name, long? excludeId) =>
        dbContext.Notes.Any(x =>
            x.IsActive
            && x.Name.Equals(name)
            && x.CreatedBy == userId
            && (excludeId == null || x.Id != excludeId));

    /// <inheritdoc />
    public Dictionary<long, string> GetNotesIdentifiers(long contextUserId) =>
        dbContext.Notes
            .Where(x => x.CreatedBy == contextUserId && x.IsActive)
            .ToDictionary(x => x.Id, x => x.Name);
}
