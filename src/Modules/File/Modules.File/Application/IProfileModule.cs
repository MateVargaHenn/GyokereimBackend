namespace Modules.File.Application;

public interface IProfileModule
{
    Task<FileResultDto?> GetProfileImageAsync(int Id);
}
