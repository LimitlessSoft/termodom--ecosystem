namespace DBMigration
{
    public interface IMigrationManager
    {
        public Task MigrateAsync();
    }
}
