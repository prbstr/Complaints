using Complaints.Data.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Complaints.UnitTests
{
    public static class DbFixtureProvider
    {
        public static DbContextOptions<ComplaintsContext> CreateNewContextOptions()
        {
            // Create a fresh service provider, and therefore a fresh 
            // InMemory database instance.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider.
            var builder = new DbContextOptionsBuilder<ComplaintsContext>();
            builder.UseInMemoryDatabase("testDb")
                   .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }
    }
}
