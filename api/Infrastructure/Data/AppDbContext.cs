using Microsoft.EntityFrameworkCore;

namespace TestDevBackJR.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options);