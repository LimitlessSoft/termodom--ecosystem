namespace TD.WebshopListener.Contracts.IManagers
{
    public interface IWorkManager
    {
        void PretvoriUProracun(int porudzbinaId);
        Task StartListeningWebshopAkcAsync();
    }
}
