using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.Interfaces.IRepositories;
using LSCore.Contracts.Exceptions;

namespace TD.Komercijalno.Repository.Repositories;
public class KomentarRepository(KomercijalnoDbContext dbContext) 
    : IKomentarRepository
{
    /// <summary>
    /// Clears all comments (internal, private, and public) for a specified document.
    /// </summary>
    /// <param name="vrDok"></param>
    /// <param name="brDok"></param>
    public void Flush(int vrDok, int brDok)
    {
        var entity = Get(vrDok, brDok);

        entity.InterniKomentar = null;
        entity.PrivatniKomentar = null;
        entity.JavniKomentar = null;

        dbContext.SaveChanges();
    }

    public Komentar Get(int vrDok, int brDok)
    {
        var entity = dbContext.Komentari.FirstOrDefault(x => x.VrDok == vrDok && x.BrDok == brDok);
        if(entity == null)
            throw new LSCoreNotFoundException();
        return entity;
    }

}
