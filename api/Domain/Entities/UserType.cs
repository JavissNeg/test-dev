using System.ComponentModel.DataAnnotations;

namespace TestDevBackJR.Domain.Entities;

public class UserType
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } 

    [MaxLength(255)]
    public string Description { get; set; }

    [Required]
    public int Status { get; set; }

    public ICollection<User> Users { get; set; } = new List<User>();
}

