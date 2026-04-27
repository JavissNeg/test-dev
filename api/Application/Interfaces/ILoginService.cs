using TestDevBackJR.Application.DTOs;
using TestDevBackJR.Domain.Entities;

namespace TestDevBackJR.Application.Interfaces;

public interface ILoginService
{
    Task<IEnumerable<Login>> GetAll();
    Task<Login> Create(LoginDto dto);
    Task<bool> Update(int id, LoginDto dto);
    Task<bool> Delete(int id);
    Task<List<UserReportDto>> GenerateUserReport();
}