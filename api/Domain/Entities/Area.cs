using System.ComponentModel.DataAnnotations;

namespace TestDevBackJR.Domain.Entities;

public class Area
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [Required]
    public int Status { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    public ICollection<User> Users { get; set; } = new List<User>();
}