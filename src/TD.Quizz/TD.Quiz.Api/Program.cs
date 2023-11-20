
using System.Security.Cryptography.X509Certificates;
using TD.Core.Framework.Extensions;

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