using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Driver;
using ShareSafe.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareSafe.Tests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>,IAsyncLifetime
    {
        private readonly TestcontainerDatabase _database = new TestcontainersBuilder<MongoDbTestcontainer>()
        .WithDatabase(new MongoDbTestcontainerConfiguration
        {
            Database = "Files",
            Username= "admin",
            Password= "password",
        }).Build();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
        
            builder.ConfigureTestServices(Services =>
            {
                Services.RemoveAll(typeof(IMongoClient));

                Services.AddSingleton<IMongoClient, MongoClient>(s =>
                {
                    var mongoDb = new MongoClient(_database.ConnectionString);             
                    return mongoDb;
                });
            });
        }


        public void ResetDatabase()
        {
            var mongoDb = new MongoClient(_database.ConnectionString);
            mongoDb.DropDatabase("Files");
        }
        

        public async Task InitializeAsync()
        {
           await _database.StartAsync();
        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            await _database.StopAsync();
        }
    }
}
