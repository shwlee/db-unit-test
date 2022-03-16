using InMemorySqliteTest.Contexts;
using InMemorySqliteTest.Extensions;
using Microsoft.Extensions.Hosting;

// host sample.
var host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((builderContext, config) =>
            {
                var env = builderContext.HostingEnvironment;
                config.SetAppSetings(env); // Load config/appsettings.json
            })
            .ConfigureServices((hostContext, services) =>
            {
                var env = hostContext.HostingEnvironment;
                var config = hostContext.Configuration;

                services.SetDbContext<BloggingContext>(config);
            })
            .Build();


// this is not executable project.
// just do the test!