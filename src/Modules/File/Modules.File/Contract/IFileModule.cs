namespace Modules.File.Contract;

public interface IFileModule
{
    Task<int> SaveFileAsync(
        FileUpload file,
        CancellationToken cancellationToken = default);
}
