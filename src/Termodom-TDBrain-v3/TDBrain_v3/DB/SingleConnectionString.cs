namespace TDBrain_v3.DB
{
    public class SingleConnectionString
    {
        private string? _path { get; set; }

        public void SetPath(string? value)
        {
            _path = value;
        }
        public string? Path()
        {
            return _path;
        }
        public string ConnectionString()
        {
            if (string.IsNullOrWhiteSpace(_path))
                throw new Exception("SingleConnectionString.Path is undefined!");

            return $"data source={Settings.ServerName}; initial catalog = {_path}; user={Settings.User}; password={Settings.Password}";
        }
    }
}
