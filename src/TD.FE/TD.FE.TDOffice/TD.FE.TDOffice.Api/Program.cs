
using TD.Core.Framework.Extensions;

namespace TD.FE.TDOffice.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            StartupExtensions.CreateTDBuilder<Startup>(args);
        }
    }
}