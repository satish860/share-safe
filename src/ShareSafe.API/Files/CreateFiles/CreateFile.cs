namespace ShareSafe.API.Files.CreateFiles
{
    public class CreateFile
    {
        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? OwnedBy { get; set; }

        public string? Signature { get; set; }
    }


    public class CreateFileValidator : Validator<CreateFile>
    {
        public CreateFileValidator()
        {
            RuleFor(p => p.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("File Name is Mandatory")
                .Must(p => IsValidFileName(p))
                .WithMessage("File Name is not valid. Should Contain extension");

        }

        public bool IsValidFileName(string fileName)
        {
            return !string.IsNullOrEmpty(Path.GetExtension(fileName));
        }
    }
}
