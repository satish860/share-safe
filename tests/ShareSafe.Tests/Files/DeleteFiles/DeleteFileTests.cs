using FluentAssertions;
using ShareSafe.API.Files.CreateFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareSafe.Tests.Files.DeleteFiles
{
    public class DeleteFileTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory customWebApplicationFactory;

        public DeleteFileTests(CustomWebApplicationFactory customWebApplicationFactory)
        {
            this.customWebApplicationFactory = customWebApplicationFactory;
        }

        [Fact]
        public async Task Should_Get_Error_If_the_File_Id_is_Not_available()
        {
            var client = this.customWebApplicationFactory.CreateClient();
            var response = await client.DeleteAsync("/files/213112");
            response.Should().Be400BadRequest();
        }

        [Fact]
        public async Task Should_Be_Able_to_Get_Accepted_When_trying_to_Delete()
        {
            var client = customWebApplicationFactory.CreateClient();

            var createResponse = await client.PostAsJsonAsync<CreateFile>("/files", new CreateFile
            {
                Description = "Test File",
                Name = "Name",
            });

            var response = await client.DeleteAsync(createResponse.Headers.Location);
            response.Should().Be202Accepted();
        }
    }
}
