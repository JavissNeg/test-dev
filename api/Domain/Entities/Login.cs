using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestDevBackJR.Domain.Entities;

public class Login
{
    public int Id { get; set; }

    [Required]
    [ForeignKey(nameof(User))]
    public int UserId { get; set; }
    public User User { get; set; }

    [Required]
    public int Extension { get; set; }

    [Required]
    public int MovementType { get; set; } // 1 = login, 0 = logout

    [Required]
    public DateTime Date { get; set; }
}