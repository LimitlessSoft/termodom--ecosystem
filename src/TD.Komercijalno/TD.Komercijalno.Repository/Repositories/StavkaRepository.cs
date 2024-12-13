using TD.Komercijalno.Contracts.Interfaces.IRepositories;

namespace TD.Komercijalno.Repository.Repositories;
public class StavkaRepository(KomercijalnoDbContext dbContext)
    : IStavkaRepository
{

}
