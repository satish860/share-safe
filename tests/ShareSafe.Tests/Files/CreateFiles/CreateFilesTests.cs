using ShareSafe.API.Files.CreateFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

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
            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }
    }
}
