using Microsoft.Extensions.Configuration;

namespace TD.Core.Framework
{
    public interface IBaseStartup
    {
        IConfigurationRoot ConfigurationRoot { get; set; }
    }
}
