using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.Interfaces.IRepositories;
using TD.Komercijalno.Contracts.Requests.Komentari;
using LSCore.Contracts.Exceptions;

namespace TD.Komercijalno.Repository.Repositories;
public class KomentarRepository(KomercijalnoDbContext dbContext) 
    : IKomentarRepository
{
    public Komentar Get(GetKomentarRequest request)
    {
        var entity = dbContext.Komentari.FirstOrDefault(x => x.VrDok == request.VrDok && x.BrDok == request.BrDok);
        if(entity == null)
            throw new LSCoreNotFoundException();
        return entity;
    }
}
