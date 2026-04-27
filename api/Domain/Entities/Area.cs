using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestDevBackJR.Domain.Entities;

public class Area
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [Required]
    [ForeignKey(nameof(AreaStatus))]
    public int AreaStatusId { get; set; }
    public AreaStatus AreaStatus { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    public ICollection<User> Users { get; set; } = new List<User>();
}