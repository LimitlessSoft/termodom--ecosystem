using TD.Core.Framework.Extensions;

namespace TD.Backuper.Sender.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            StartupExtensions.CreateTDBuilder<Startup>(args);
        }
    }
}