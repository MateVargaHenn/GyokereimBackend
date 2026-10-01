namespace Modules.File.Infrastructure;

internal class ProfileModule(FileDbContext context) : IProfileModule
{
    private readonly FileDbContext _context = context;
    public Task<FileResultDto?> GetProfileImageAsync(int Id)
    {
        Domain.File? file = _context.Files.FirstOrDefault(f => f.Id == Id);
        if (file == null)
        {
            return Task.FromResult<FileResultDto?>(null);
        }
        //var fileResult = new FileResultDto
        //{
        //    Content = new MemoryStream(file.Content),
        //    ContentType = file.ContentType,
        //    FileName = file.FileName
        //};
        return Task.FromResult<FileResultDto?>(null); 
    }
}
