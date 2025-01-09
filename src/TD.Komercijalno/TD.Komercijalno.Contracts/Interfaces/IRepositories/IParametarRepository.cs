using TD.Komercijalno.Contracts.Entities;

namespace TD.Komercijalno.Contracts.Interfaces.IRepositories;

public interface IParametarRepository
{
    Parametar Get(string naziv);
    Parametar? GetOrDefault(string naziv);
    void SetVrednost(string naziv, string vrednost);
}