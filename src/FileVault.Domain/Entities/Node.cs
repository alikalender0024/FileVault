namespace FileVault.Domain.Entities;

public class Node
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public Guid OwnerId { get; set; }
    public User Owner { get; set; } = null!;

    /// <summary>
    /// Üst klasörün ID'si. Eğer NULL ise kullanıcı kök (root) dizinindedir.
    /// </summary>
    public Guid? ParentId { get; set; }
    public Node? Parent { get; set; }

    public string Name { get; set; } = string.Empty;
    public bool IsFolder { get; set; }

    /// <summary>
    /// Eğer IsFolder == false ise, fiziki disktaki karşılığı olan Blob ID.
    /// </summary>
    public Guid? BlobId { get; set; }

    /// <summary>
    /// Dosya boyutu (Byte). Klasörler için 0.
    /// </summary>
    public long Size { get; set; }

    /// <summary>
    /// Bütünlük kontrolü ve de-duplication potansiyeli için SHA-256 hash değeri.
    /// </summary>
    public string? Sha256 { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Soft-delete (Çöp Kutusu) mekanizması için silinme tarihi.
    /// </summary>
    public DateTime? DeletedAt { get; set; }

    // Navigation properties (Hiyerarşik Adjacency List)
    public ICollection<Node> Children { get; set; } = new List<Node>();
}