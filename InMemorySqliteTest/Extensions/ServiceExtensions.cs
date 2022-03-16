using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InMemorySqliteTest.Extensions;
public static class ServiceExtensions
{
    public static void SetDbContext<T>(this IServiceCollection services, IConfiguration config)
        where T : DbContext
    {
        var connectionString = config.GetConnectionString("ConnectionStrings:DefaultConnectionString");

        services.AddDbContextFactory<T>(options => options.UseSqlServer(connectionString));
    }
}
