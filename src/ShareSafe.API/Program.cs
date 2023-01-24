global using FastEndpoints;
using Amazon.Runtime;
using Amazon.S3;
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
            ConfigureMongoDB(builder);
            builder.Services.AddSingleton<IAmazonS3>(s =>
            {
                var configuration = s.GetRequiredService<IConfiguration>();
                var Config = new AmazonS3Config
                {
                    ServiceURL = configuration["DOCTL:ServiceURL"],
                    ForcePathStyle = false,
                };
                var awsAccessKey = configuration["AWS_ACCESS_KEY"];
                var awsSecret = configuration["AWS_SECRET"];
                var cred = new BasicAWSCredentials(awsAccessKey, awsSecret);
                return new AmazonS3Client(cred, Config);

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

        private static void ConfigureMongoDB(WebApplicationBuilder builder)
        {
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
        }
    }
}