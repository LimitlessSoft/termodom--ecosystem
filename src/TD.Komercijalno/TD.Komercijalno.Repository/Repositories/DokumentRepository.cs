using LSCore.Contracts.Exceptions;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.Interfaces.IRepositories;

namespace TD.Komercijalno.Repository.Repositories;

public class DokumentRepository(KomercijalnoDbContext dbContext) : IDokumentRepository
{
    public Dokument Get(int vrDok, int brDok)
    {
        var entity = GetOrDefault(vrDok, brDok);
        if (entity == null)
            throw new LSCoreNotFoundException();
        
        return entity;
    }

    public Dokument? GetOrDefault(int vrDok, int brDok) =>
        dbContext.Dokumenti.FirstOrDefault(x =>
            x.VrDok == vrDok && x.BrDok == brDok
        );
}