using System.ComponentModel.DataAnnotations;

namespace TestDevBackJR.Domain.Entities;

public class UserStatus
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } // e.g., "Active", "Inactive", "Suspended"

    [MaxLength(255)]
    public string Description { get; set; }

    [Required]
    public int Status { get; set; } // 1 = Enabled, 0 = Disabled

    public ICollection<User> Users { get; set; } = new List<User>();
}

