using Microsoft.EntityFrameworkCore;
using TestDevBackJR.Application.DTOs;
using TestDevBackJR.Application.Interfaces;
using TestDevBackJR.Application.Validators;
using TestDevBackJR.Domain.Entities;
using TestDevBackJR.Infrastructure.Data;

namespace TestDevBackJR.Application.Services;

public class LoginService(AppDbContext context, LoginValidator validator) : ILoginService
{
    public async Task<IEnumerable<Login>> GetAll()
    {
        return await context.Logins
            .Include(l => l.User)
            .ToListAsync();
    }

    public async Task<Login> Create(LoginDto dto)
    {
        await validator.Validate(dto);

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

        await validator.Validate(dto, id);

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
}