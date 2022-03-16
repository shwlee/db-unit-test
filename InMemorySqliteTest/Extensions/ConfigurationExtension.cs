using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace InMemorySqliteTest.Extensions;
public static class ConfigurationExtension
{
    public static void SetAppSettingsJson(this IConfigurationBuilder config, string environmentName, string folder = "")
    {
        config
          .AddJsonFile(Path.Combine(folder, "appsettings.json"), optional: true, reloadOnChange: true)
          .AddJsonFile(Path.Combine(folder, $"appsettings.{environmentName.ToLower()}.json"), optional: true);
    }

    public static void SetAppSetings(this IConfigurationBuilder config, IHostEnvironment env)
    {
        config.SetAppSettingsJson(env.EnvironmentName, "config");
    }
}
