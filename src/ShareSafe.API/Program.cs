global using FastEndpoints;
using MongoDB.Driver;
using ShareSafe.API.Files;
using System;

namespace ShareSafe.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddEnvironmentVariables();
            builder.Services.AddSingleton<IMongoClient, MongoClient>(s =>
            {
                var uri = s.GetRequiredService<IConfiguration>()["DBHOST"];
                return new MongoClient(uri);
            });
            builder.Services.AddScoped<IMongoCollection<FileMetadata>>(s =>
            {
                var mongoClient = s.GetRequiredService<IMongoClient>();
                var DBName = s.GetRequiredService<IConfiguration>()["DBNAME"];
                var database = mongoClient.GetDatabase(DBName);
                var collection = database.GetCollection<FileMetadata>("files");
                return collection;
            });
            builder.Services.AddFastEndpoints();
            builder.WebHost.ConfigureKestrel(o =>
            {
                o.Limits.MaxRequestBodySize = 1073741824; //set to max allowed file size of your system
            });
            var app = builder.Build();

            app.UseAuthorization();
            app.UseFastEndpoints();
            app.MapGet("/", () => "Hello World!");

            app.Run();
        }
    }
}