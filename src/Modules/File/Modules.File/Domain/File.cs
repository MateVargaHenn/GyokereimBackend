namespace Modules.File.Domain;

internal sealed class File : Entity
{
    internal string Key { get; private set; } = default!;
    internal string FileName { get; private set; } = default!;
    internal string ContentType { get; private set; } = default!;
    internal long Size { get; private set; }
    internal string StoragePath { get; private set; } = default!;

    internal string? Title { get; private set; }
    internal string? AltText { get; private set; }

    internal bool IsPublic { get; private set; }

    private File()
    {
    }

    internal static File Create(
        string key,
        string fileName,
        string contentType,
        long size,
        string storagePath) => new File
        {
            Key = key,
            FileName = fileName,
            ContentType = contentType,
            Size = size,
            StoragePath = storagePath,
            IsPublic = false
        };

    internal void Publish() => IsPublic = true;

    internal void Unpublish() => IsPublic = false;

    internal void ChangeMetadata(
    string? title,
    string? altText)
    {
        Title = string.IsNullOrWhiteSpace(title)
            ? null
            : title.Trim();

        AltText = string.IsNullOrWhiteSpace(altText)
            ? null
            : altText.Trim();
    }
}
