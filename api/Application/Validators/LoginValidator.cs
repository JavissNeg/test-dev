using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TestDevBackJR.Application.DTOs;
using TestDevBackJR.Infrastructure.Data;

namespace TestDevBackJR.Application.Validators;

public class LoginValidator : AbstractValidator<LoginDto>
{
    private readonly AppDbContext _context;
    private const string ExcludeIdContextKey = "ExcludeId";

    public LoginValidator(AppDbContext context)
    {
        _context = context;

        RuleFor(x => x.MovementType)
            .Must(m => m == 0 || m == 1)
            .WithMessage("MovementType debe ser 0 (logout) o 1 (login)");

        RuleFor(x => x.UserId)
            .MustAsync(async (userId, cancellation) =>
                await _context.Users.AnyAsync(u => u.Id == userId, cancellation))
            .WithMessage("Usuario con ID {PropertyValue} no existe");

        RuleFor(x => x.Date)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("La fecha no puede estar en el futuro");

        RuleFor(x => x)
            .CustomAsync(async (dto, validationContext, cancellation) =>
            {
                var sequenceFailure = await GetSequenceValidationFailure(dto, validationContext, cancellation);
                if (sequenceFailure.HasValue)
                    validationContext.AddFailure(sequenceFailure.Value.PropertyName, sequenceFailure.Value.Message);
            });
    }

    private async Task<(string PropertyName, string Message)?> GetSequenceValidationFailure(
        LoginDto dto,
        ValidationContext<LoginDto> validationContext,
        CancellationToken cancellation)
    {
        var excludeId = validationContext.RootContextData.TryGetValue(ExcludeIdContextKey, out var rawExcludeId) &&
                        rawExcludeId is int parsedExcludeId
            ? parsedExcludeId
            : (int?)null;

        var lastLogin = await _context.Logins
            .AsNoTracking()
            .Where(l => l.UserId == dto.UserId &&
                        (excludeId == null || l.Id != excludeId))
            .OrderByDescending(l => l.Date)
            .ThenByDescending(l => l.Id)
            .FirstOrDefaultAsync(cancellation);

        if (lastLogin == null)
        {
            if (dto.MovementType != 1)
                return (nameof(LoginDto.MovementType), "El primer movimiento del usuario debe ser login (1).");

            return null;
        }

        if (dto.Date < lastLogin.Date)
            return (nameof(LoginDto.Date), "La fecha no puede ser anterior al ultimo movimiento registrado.");

        if (dto.MovementType == lastLogin.MovementType)
        {
            var message = dto.MovementType == 1
                ? "No puede registrar login consecutivo; primero debe registrar logout (0)."
                : "No puede registrar logout consecutivo; primero debe registrar login (1).";

            return (nameof(LoginDto.MovementType), message);
        }

        return null;
    }
}