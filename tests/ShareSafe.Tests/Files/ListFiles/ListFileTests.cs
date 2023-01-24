using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions.Web;
using ShareSafe.API.Files.CreateFiles;
using ShareSafe.API.Files.ListFIles;
using System.Xml.Linq;

namespace ShareSafe.Tests.Files.ListFiles
{
    public class ListFileTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory customWebApplicationFactory;

        public ListFileTests(CustomWebApplicationFactory customWebApplicationFactory)
        {
            this.customWebApplicationFactory = customWebApplicationFactory;
        }

        [Fact]
        public async Task Should_Return_404_When_there_Are_No_Files()
        {
            customWebApplicationFactory.ResetDatabase();
            var client = customWebApplicationFactory.CreateClient();
            var response = await client.GetAsync("/files");
            response.Should().Be404NotFound();
        }

        [Fact]
        public async Task Should_Get_the_Saved_File_metadata_When_List_is_asked()
        {
            var client = customWebApplicationFactory.CreateClient();
           
            var createResponse = await client.PostAsJsonAsync<CreateFile>("/files", new CreateFile
            {
                Description = "Test File",
                Name = "Name.txt",
            });
            var response = await client.GetAsync("/files");
            response.Should().Be200Ok();
            response.Should()
                    .Satisfy<IEnumerable<ListFileResponse>>(model =>
                                                            model.Should()
                                                            .HaveCount(1));
        }
    }
}
