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
