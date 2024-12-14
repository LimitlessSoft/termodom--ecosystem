using Microsoft.EntityFrameworkCore;
using TD.Komercijalno.Contracts.Interfaces.IRepositories;

namespace TD.Komercijalno.Repository.Repositories;

public class StavkaRepository(KomercijalnoDbContext dbContext)
    : IStavkaRepository
{
    /// <summary>
    /// Deletes all Stavke with the given VrDok and BrDok.
    /// </summary>
    /// <param name="vrDok"></param>
    /// <param name="brDok"></param>
    public void Delete(int vrDok, int brDok)
    {
        dbContext.Stavke
            .Where(x => x.VrDok == vrDok && x.BrDok == brDok)
            .ExecuteDelete();
        dbContext.SaveChanges();
    }
}
