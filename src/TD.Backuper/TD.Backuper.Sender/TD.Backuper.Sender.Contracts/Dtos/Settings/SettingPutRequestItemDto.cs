namespace TD.Backuper.Sender.Contracts.Dtos.Settings
{
    public class SettingPutRequestItemDto
    {
        public string Path { get; set; }
        public long IntervalMinutes { get; set; }
    }
}
