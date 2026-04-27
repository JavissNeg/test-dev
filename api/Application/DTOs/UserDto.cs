namespace TestDevBackJR.Application.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string SecondLastName { get; set; }
    public int UserTypeId { get; set; }
    public string UserTypeName { get; set; }
    public int UserStatusId { get; set; }
    public string UserStatusName { get; set; }
    public int AreaId { get; set; }
    public string AreaName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAttempt { get; set; }
}

