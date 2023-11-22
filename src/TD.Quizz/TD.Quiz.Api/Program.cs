
using System.Security.Cryptography.X509Certificates;
using LSCore.Framework.Extensions;

namespace TD.Quiz.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LSCoreStartupExtensions.CreateTDBuilder<Startup>(args);
        }
    }
}