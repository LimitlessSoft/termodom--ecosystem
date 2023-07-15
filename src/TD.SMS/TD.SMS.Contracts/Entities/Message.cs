using TD.SMS.Contracts.Enums;

namespace TD.SMS.Contracts.Entities
{
    public class Message
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public string Mogile { get; set; }
        public MessageType Type { get; set; }
        public DateTime Date { get; set; }
    }
}
