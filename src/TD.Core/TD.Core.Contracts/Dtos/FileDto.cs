namespace TD.Core.Contracts.Dtos
{
    public class FileDto
    {
        public string ContentType { get; set; }
        public int ContentLength { get => Data == null ? 0 : Data.Length; }
        public byte[] Data { get; set; }
        public Dictionary<string, string> Tags { get; set; } = new Dictionary<string, string>();
    }
}
