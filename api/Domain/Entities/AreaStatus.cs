using System.ComponentModel.DataAnnotations;

namespace TestDevBackJR.Domain.Entities;

public class AreaStatus
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } // e.g., "Active", "Inactive"

    [MaxLength(255)]
    public string Description { get; set; }

    [Required]
    public int Status { get; set; } // 1 = Enabled, 0 = Disabled

    public ICollection<Area> Areas { get; set; } = new List<Area>();
}

