using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestDevBackJR.Domain.Entities;

public class User
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Username { get; set; }

    [Required]
    [MaxLength(255)]
    public string Password { get; set; }

    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; }

    [MaxLength(100)]
    public string SecondLastName { get; set; }

    [Required]
    public int UserTypeId { get; set; }

    [Required]
    public int Status { get; set; }

    [Required]
    [ForeignKey(nameof(Area))]
    public int AreaId { get; set; }
    public Area Area { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? LastLoginAttempt { get; set; }

    public ICollection<Login> Logins { get; set; } = new List<Login>();
}