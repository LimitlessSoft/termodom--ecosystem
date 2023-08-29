using Microsoft.AspNetCore.Hosting;
using TD.Core.Framework.Extensions;

namespace TD.Web.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            StartupExtensions.CreateTDBuilder<Startup>(args);
        }
    }
}