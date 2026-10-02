namespace FileVault.Domain.Entities;

public enum UserRole
{
    Member = 0,
    Admin = 1
}

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.Member;
    
    /// <summary>
    /// Kullanıcının maksimum depolama kotası (Byte cinsinden). Örn: 50 GB = 53,687,091,200 bytes
    /// </summary>
    public long QuotaBytes { get; set; }
    
    /// <summary>
    /// Kullanıcının o an kullandığı toplam alan (Byte cinsinden).
    /// Node eklendikçe/silindikçe güncellenir.
    /// </summary>
    public long UsedBytes { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<Node> Nodes { get; set; } = new List<Node>();
}