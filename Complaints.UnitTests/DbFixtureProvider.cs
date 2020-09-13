using Complaints.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Complaints.UnitTests
{
    public class DbFixture
    {
        public ServiceProvider ServiceProvider { get; private set; }
        public DbFixture()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDbContext<ComplaintsContext>(options =>
                options.UseInMemoryDatabase("testDb"),
                ServiceLifetime.Transient
            );

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}
