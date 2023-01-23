namespace ShareSafe.API.Files.UploadFile
{
    public class UploadFileEndpoint : EndpointWithoutRequest
    {
        public override void Configure()
        {
            Post("/files/{fileId}/upload");
            AllowFileUploads(dontAutoBindFormData: true);
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            await foreach (var section in FormFileSectionsAsync(ct))
            {
                if (section is not null)
                {
                    using (var fs = System.IO.File.Create(section.FileName))
                    {
                        await section.Section.Body.CopyToAsync(fs, 1024 * 64, ct);
                    }
                }
                await SendNoContentAsync(ct);
            }
        }
    }
}
