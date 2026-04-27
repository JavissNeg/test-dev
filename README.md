# Test Dev - API  

API REST para gestión de logins y sesiones de usuarios.

---

## 🛠️ Stack tecnológico

- **.NET 10** - Framework backend
- **Entity Framework Core** - ORM
- **SQL Server** - Base de datos
- **xUnit** - Testing
- **Moq** - Mocking
- **Docker/Podman** - Containerización

---

## 📋 Requisitos previos

- .NET 10 SDK
- Docker o Podman
- Puerto 7151 disponible (API)
- Puerto 1433 disponible (SQL Server)

---

## 🚀 Instalación

### 1. Clonar repositorio
```bash
git clone https://github.com/JavissNeg/test-dev.git
cd testdevbackjr
```

### 2. Levantar SQL Server

**Docker:**
```bash
docker compose up -d sqlserver
```

**Podman:**
```bash
podman compose up -d sqlserver
```

### 3. Ejecutar API
```bash
cd api
dotnet run
```

API disponible en: `https://localhost:7151`

---

## 🗄️ Conexión a base de datos

| Parámetro | Valor |
|-----------|-------|
| Host | `localhost:1433` |
| Usuario | `sa` |
| Contraseña | `P4ssw@rd` |
| Database | Generada automáticamente |

### Scripts SQL

| Script     | Uso |
|------------|-----|
| `seed.sql` | Poblar la base de datos con datos iniciales. |
| `avg_login_time_monthly.sql` | Reporte de tiempo promedio de login por mes. |
| `max_login_time.sql` | Reporte de tiempo maximo de login. |
| `min_login_time.sql` | Reporte de tiempo minimo de login. |

---

## 📡 Endpoints

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/logins` | Obtener todos los logins |
| POST | `/logins` | Crear nuevo login |
| PUT | `/logins/{id}` | Actualizar login |
| DELETE | `/logins/{id}` | Eliminar login |
| GET | `/logins/export/csv` | Descargar reporte CSV |

### Ejemplo
```http
POST https://localhost:7151/logins
Content-Type: application/json

{
  "userId": 1,
  "extension": 100,
  "movementType": 1,
  "date": "2026-04-26T10:00:00"
}
```

### Descargar CSV

Asegúrate que la API esté ejecutándose. Luego:

```bash
curl -k -L https://localhost:7151/logins/export/csv -o user_report.csv
```

CSV incluye: Nombre de usuario, nombre completo, área, total de horas trabajadas

---

## ✅ Pruebas

```bash
dotnet test TestDevBackJR.Tests/TestDevBackJR.Tests.csproj
```

Prueba específica:
```bash
dotnet test --filter "FullyQualifiedName~LoginsControllerTests.Get_ReturnsOkResult_WithLoginsList"
```

---

## 🐳 Orquestación completa

**Docker:**
```bash
docker compose up -d --build
```

**Podman:**
```bash
podman compose up -d --build
```

Detener:
```bash
docker compose down
# o
podman compose down
```

