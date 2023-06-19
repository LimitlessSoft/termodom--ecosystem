namespace TD.WebshopListener.Contracts.IManagers
{
    public interface IWorkManager
    {
        void PretvoriUDokument(int porudzbinaId, short vrDok);
        Task StartListeningWebshopAkcAsync();
    }
}
