using FileVault.Application.Common.Interfaces;

namespace FileVault.Infrastructure.Storage;

public class LocalDiskBlobStorage : IBlobStorage
{
    private readonly string _baseStoragePath;

    public LocalDiskBlobStorage(string baseStoragePath)
    {
        _baseStoragePath = baseStoragePath;
        if (!Directory.Exists(_baseStoragePath))
        {
            Directory.CreateDirectory(_baseStoragePath);
        }
    }

    public async Task<Guid> SaveAsync(Stream content, CancellationToken ct = default)
    {
        var blobId = Guid.NewGuid();
        var filePath = GetFilePath(blobId);

        // Alt klasörü oluştur (örn: /var/filevault/blobs/ab/)
        var directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        // Buffer taşması yaşanmaması için FileStream kullanarak diske yazıyoruz
        await using var fileStream = new FileStream(
            filePath, 
            FileMode.CreateNew, 
            FileAccess.Write, 
            FileShare.None, 
            bufferSize: 81920, // 80 KB Buffer
            useAsync: true);

        await content.CopyToAsync(fileStream, ct);
        return blobId;
    }

    public Task<Stream> OpenReadAsync(Guid blobId, CancellationToken ct = default)
    {
        var filePath = GetFilePath(blobId);
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Blob file not found: {blobId}");
        }

        Stream stream = new FileStream(
            filePath, 
            FileMode.Open, 
            FileAccess.Read, 
            FileShare.Read, 
            bufferSize: 81920, 
            useAsync: true);

        return Task.FromResult(stream);
    }

    public Task DeleteAsync(Guid blobId, CancellationToken ct = default)
    {
        var filePath = GetFilePath(blobId);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        return Task.CompletedTask;
    }

    /// <summary>
    /// Dosya sistemindeki yığılmayı önlemek için GUID'in ilk 2 karakterini alt klasör olarak kullanır.
    /// Örn: blobId = "ab35f9..." -> "/storage/blobs/ab/ab35f9..."
    /// </summary>
    private string GetFilePath(Guid blobId)
    {
        var idStr = blobId.ToString("N");
        var prefix = idStr.Substring(0, 2);
        return Path.Combine(_baseStoragePath, prefix, idStr);
    }
}