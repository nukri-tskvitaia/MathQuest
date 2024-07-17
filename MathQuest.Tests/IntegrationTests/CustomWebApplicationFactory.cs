using Data.Data;
using MathQuestWebApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MathQuest.Tests.IntegrationTests
{
    internal class CustomWebApplicationFactory : WebApplicationFactory<Startup>
    {
        private readonly Action<IServiceCollection> _configureServices;

        public CustomWebApplicationFactory(Action<IServiceCollection> configureServices = null)
        {
            _configureServices = configureServices;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                RemoveLibraryDbContextRegistration(services);

                var serviceProvider = GetInMemoryServiceProvider();

                services.AddDbContextPool<MathQuestDbContext>(options =>
                {
                    options.UseInMemoryDatabase(Guid.Empty.ToString());
                    options.UseInternalServiceProvider(serviceProvider);
                });

                _configureServices?.Invoke(services);

                using var scope = services.BuildServiceProvider().CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<MathQuestDbContext>();

                UnitTestHelper.SeedData(context);
            });
        }

        private static ServiceProvider GetInMemoryServiceProvider()
        {
            return new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();
        }

        private static void RemoveLibraryDbContextRegistration(IServiceCollection services)
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<MathQuestDbContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
        }
    }
}