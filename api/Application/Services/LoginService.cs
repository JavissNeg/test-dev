using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TestDevBackJR.Application.DTOs;
using TestDevBackJR.Application.Interfaces;
using TestDevBackJR.Domain.Entities;
using TestDevBackJR.Infrastructure.Data;

namespace TestDevBackJR.Application.Services;

public class LoginService(AppDbContext context, IValidator<LoginDto> validator) : ILoginService
{
    public async Task<IEnumerable<Login>> GetAll()
    {
        return await context.Logins
            .Include(l => l.User)
            .ToListAsync();
    }

    public async Task<Login> Create(LoginDto dto)
    {
        var result = await validator.ValidateAsync(dto);
        if (!result.IsValid)
            throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.ErrorMessage)));

        var login = new Login
        {
            UserId = dto.UserId,
            Extension = dto.Extension,
            MovementType = dto.MovementType,
            Date = dto.Date
        };

        context.Logins.Add(login);
        await context.SaveChangesAsync();

        return login;
    }

    public async Task<bool> Update(int id, LoginDto dto)
    {
        var login = await context.Logins.FindAsync(id);
        if (login == null)
            return false;

        var validationContext = new ValidationContext<LoginDto>(dto);
        validationContext.RootContextData["ExcludeId"] = id;

        var result = await validator.ValidateAsync(validationContext);
        if (!result.IsValid)
            throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.ErrorMessage)));

        login.UserId = dto.UserId;
        login.Extension = dto.Extension;
        login.MovementType = dto.MovementType;
        login.Date = dto.Date;

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Delete(int id)
    {
        var login = await context.Logins.FindAsync(id);
        if (login == null)
            return false;

        context.Logins.Remove(login);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<List<UserReportDto>> GenerateUserReport()
    {
        var users = await context.Users
            .Include(u => u.Logins)
            .Include(u => u.Area)
            .ToListAsync();

        var report = new List<UserReportDto>();

        foreach (var user in users)
        {
            var logins = user.Logins.OrderBy(l => l.Date).ToList();
            decimal totalHours = 0;

            for (int i = 0; i < logins.Count - 1; i++)
            {
                if (logins[i].MovementType == 1 && logins[i + 1].MovementType == 0)
                {
                    var duration = logins[i + 1].Date - logins[i].Date;
                    totalHours += (decimal)duration.TotalHours;
                }
            }

            report.Add(new UserReportDto
            {
                Username = user.Username,
                FullName = $"{user.FirstName} {user.LastName} {user.SecondLastName}".Trim(),
                Area = user.Area?.Name ?? "N/A",
                TotalHours = Math.Round(totalHours, 2)
            });
        }

        return report;
    }
}
