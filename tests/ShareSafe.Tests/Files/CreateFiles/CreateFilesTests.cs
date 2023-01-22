using FluentAssertions;
using ShareSafe.API.Files.CreateFiles;
using System.Net.Http.Json;
using FluentAssertions.Web;
using ShareSafe.API.Files.GetFile;

namespace ShareSafe.Tests.Files.CreateFiles
{
    public class CreateFilesTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory customWebApplicationFactory;

        public CreateFilesTests(CustomWebApplicationFactory customWebApplicationFactory)
        {
            this.customWebApplicationFactory = customWebApplicationFactory;
        }

        [Fact]
        public async Task Should_be_able_to_Create_the_Files()
        {
            var client = this.customWebApplicationFactory.CreateClient();
            var response = await client.PostAsJsonAsync<CreateFile>("/files", new CreateFile
            {
                Description = "Test File",
                Name = "Name",
            });
            response.Should()
                    .Be201Created().And
                    .HaveHeader("Location");
            
        }

        [Fact]
        public async Task Should_be_Able_to_Create_And_Check_the_Get()
        {
            var expectedfileMetadata = new CreateFile
            {
                Description = "Test File",
                Name = "Name",
            };
            var client = this.customWebApplicationFactory.CreateClient();
            var response = await client.PostAsJsonAsync<CreateFile>("/files", expectedfileMetadata);

            var getResponse = await client.GetFromJsonAsync<GetFile>(response.Headers.Location);

            getResponse.Should()
                       .BeEquivalentTo(expectedfileMetadata);

        }
    }
}
