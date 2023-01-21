namespace ShareSafe.API.Ping
{
    public class PingEndpoint : EndpointWithoutRequest
    {
        public override void Configure()
        {
            Get("/ping");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var pong = "pong";
            await SendAsync(pong);
        }
    }
}
