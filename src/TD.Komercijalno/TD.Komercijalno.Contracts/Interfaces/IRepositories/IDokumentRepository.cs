using TD.Komercijalno.Contracts.Entities;

namespace TD.Komercijalno.Contracts.Interfaces.IRepositories;

public interface IDokumentRepository
{
    Dokument Get(int vrDok, int brDok);
    Dokument? GetOrDefault(int vrDok, int brDok);
}