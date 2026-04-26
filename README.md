# Prueba práctica en .NET y SQL Server

## Levantar servicio de SQL Server 

### Docker
```bash
docker compose up -d sqlserver
```

### Podman
```bash
podman compose up -d sqlserver
```

## Levantar todos los servicios

### Docker
```bash
docker compose up -d --build
```

### Podman
```bash
podman compose up -d  --build
```

## Migraciones

Si necesitas aplicar las migraciones manualmente:

```bash
dotnet ef database update -p api/api.csproj
```

## Pruebas Unitarias

### Ejecutar todas las pruebas

```bash
 dotnet test TestDevBackJR.Tests/TestDevBackJR.Tests.csproj
```

### Ejecutar pruebas con detalles verbosos

```bash
dotnet test -v normal
```

### Ejecutar pruebas del proyecto específicamente

```bash
dotnet test TestDevBackJR.Tests/TestDevBackJR.Tests.csproj
```

### Ejecutar una prueba específica

```bash
dotnet test --filter "FullyQualifiedName~LoginsControllerTests.Get_ReturnsOkResult_WithLoginsList"
```