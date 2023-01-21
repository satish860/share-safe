using Microsoft.AspNetCore.Mvc.Testing;
using ShareSafe.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareSafe.Tests
{
    public class PingEndpointTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> factory;

        public PingEndpointTests(WebApplicationFactory<Program> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task Should_be_able_to_hit_ping_endpoint()
        {
            var client = factory.CreateClient();

            var response = await client.GetAsync("/ping");
            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal("pong", content.Replace("\"",string.Empty));
        }
    }
}
