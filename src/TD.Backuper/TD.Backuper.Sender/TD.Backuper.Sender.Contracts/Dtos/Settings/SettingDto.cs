namespace TD.Backuper.Sender.Contracts.Dtos.Settings
{
    public class SettingDto
    {
        public string Path { get; set; }
        public long IntervalMinutes { get; set; }
        public DateTime? LastTimeBackup { get; set; }
    }
}
