namespace TDBrain_v3
{
	public static class Debug
	{
		private static List<string> _buffer { get; set; } = new List<string>();
		private static readonly int _maxSize = 4096;

		public static void Log(params string[] message)
		{
			for (int i = 0; i < message.Length; i++)
			{
				_buffer.Add($"[ {DateTime.Now.ToString("dd.MM.yyyy (HH:mm:ss)")} ] {message[i]}");
				if (_buffer.Count > _maxSize)
					_buffer.RemoveAt(0);
			}
		}

		public static List<string> Get()
		{
			List<string> b = _buffer.ToList();
			b.Reverse();
			return b;
		}
	}
}
