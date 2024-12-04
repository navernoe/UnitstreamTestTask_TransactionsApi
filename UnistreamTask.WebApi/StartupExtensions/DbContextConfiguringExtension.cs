using Microsoft.EntityFrameworkCore;
using UnistreamTask.Data;

namespace UnistreamTask.WebApi.StartupExtensions;

public static class DbContextConfiguringExtension
{
    public static void AddInMemoryDbContext(this IServiceCollection services, string dbName)
    {
        services.AddDbContext<InMemoryDbContext>(opt => opt.UseInMemoryDatabase(databaseName: dbName));
    }
}