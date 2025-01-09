using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Interfaces.IRepositories;
using TD.Komercijalno.Contracts.Requests.Parametri;

namespace TD.Komercijalno.Domain.Managers;

public class ParametarManager(IParametarRepository parametarRepository) : IParametarManager
{
    public void Update(UpdateParametarRequest request) =>
        parametarRepository.SetVrednost(request.Naziv, request.Vrednost);
}