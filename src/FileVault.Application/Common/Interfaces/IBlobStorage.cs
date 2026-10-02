namespace FileVault.Application.Common.Interfaces;

public interface IBlobStorage
{
    /// <summary>
    /// Stream verisini diske stream ederek kaydeder ve benzersiz Blob ID'yi döner.
    /// </summary>
    Task<Guid> SaveAsync(Stream content, CancellationToken ct = default);

    /// <summary>
    /// Verilen Blob ID'sine ait dosya akışını okuma modunda açar.
    /// </summary>
    Task<Stream> OpenReadAsync(Guid blobId, CancellationToken ct = default);

    /// <summary>
    /// Blob'u fiziksel depolamadan siler.
    /// </summary>
    Task DeleteAsync(Guid blobId, CancellationToken ct = default);
}