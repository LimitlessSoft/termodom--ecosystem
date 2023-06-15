namespace TD.Webshop.Contracts.Entities
{
    public class User
    {
        public long Id { get; set; }
        public string Username { get; set; } = "Unknown";
        public string Password { get; set; } = "";
        public string Firstname { get; set; } = "Unknown";
        public string? Lastname { get; set; }
        public string? Nickname { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
    }
}
