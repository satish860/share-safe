global using FastEndpoints;
using MongoDB.Driver;

namespace ShareSafe.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddSingleton<IMongoClient, MongoClient>(s =>
            {
                var uri = s.GetRequiredService<IConfiguration>()["DBHOST"];
                Console.WriteLine(uri);
                return new MongoClient(uri);
            });
            builder.Services.AddFastEndpoints();
            var app = builder.Build();

            app.UseAuthorization();
            app.UseFastEndpoints();
            app.MapGet("/", () => "Hello World!");

            app.Run();
        }
    }
}