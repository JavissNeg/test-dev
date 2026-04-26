using Microsoft.EntityFrameworkCore;
using TestDevBackJR.Application.DTOs;
using TestDevBackJR.Infrastructure.Data;

namespace TestDevBackJR.Application.Validators;

public class LoginValidator(AppDbContext context)
{
    public async Task Validate(LoginDto dto, int? excludeId = null)
    {
        // Validar MovementType (0 o 1)
        if (dto.MovementType != 0 && dto.MovementType != 1)
            throw new InvalidOperationException("MovementType debe ser 0 (logout) o 1 (login)");

        // Validar usuario existe
        var userExists = await context.Users.AnyAsync(u => u.Id == dto.UserId);
        if (!userExists)
            throw new InvalidOperationException($"Usuario con ID {dto.UserId} no existe");

        // Validar fecha no esté en el futuro
        if (dto.Date > DateTime.UtcNow)
            throw new InvalidOperationException("La fecha no puede estar en el futuro");

        // Obtener último login, excluyendo el registro siendo actualizado
        var lastLogin = await context.Logins
            .Where(l => l.UserId == dto.UserId && (excludeId == null || l.Id != excludeId))
            .OrderByDescending(l => l.Date)
            .FirstOrDefaultAsync();

        // Validar secuencia login/logout
        if (dto.MovementType == 1 && lastLogin != null && lastLogin.MovementType == 1)
            throw new InvalidOperationException("Ya existe un login sin logout anterior");

        // Validar que logout sea después del último login
        if (dto.MovementType == 0 && lastLogin != null && dto.Date < lastLogin.Date)
            throw new InvalidOperationException("La fecha del logout no puede ser anterior al último login");
    }
}
