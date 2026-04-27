-- 1. USER STATUSES (Catálogo de estados de usuario)
INSERT INTO dbo.UserStatuses (Name, Description, Status)
VALUES 
    (N'Active', N'Usuario activo y con acceso al sistema', 1),
    (N'Inactive', N'Usuario inactivo temporalmente', 1),
    (N'Suspended', N'Usuario suspendido por violación de políticas', 1);

-- 2. USER TYPES (Catálogo de roles/tipos de usuario)
INSERT INTO dbo.UserTypes (Name, Description, Status)
VALUES 
    (N'Administrator', N'Acceso total a todas las funcionalidades', 1),
    (N'Operator', N'Acceso a operaciones normales del sistema', 1),
    (N'Viewer', N'Acceso de solo lectura', 1);

-- 3. AREA STATUSES (Catálogo de estados de área)
INSERT INTO dbo.AreaStatuses (Name, Description, Status)
VALUES 
    (N'Active', N'Área operativa', 1),
    (N'Inactive', N'Área no operativa', 1);

-- ===== MASTER DATA =====

-- 4. AREAS (usando AreaStatusId en lugar de Status)
INSERT INTO dbo.Areas (Name, AreaStatusId, CreatedAt)
VALUES 
    (N'Sistemas', 1, GETUTCDATE()),
    (N'Operaciones', 1, GETUTCDATE()),
    (N'Administración', 1, GETUTCDATE());

-- 5. USERS (usando UserTypeId y UserStatusId con integridad referencial)
INSERT INTO dbo.Users
(Username, Password, FirstName, LastName, SecondLastName, UserTypeId, UserStatusId, AreaId, CreatedAt, LastLoginAttempt)
VALUES
    (N'jsmith', N'hashed_password', N'John', N'Smith', N'Doe', 1, 1, 1, GETUTCDATE(), NULL),
    (N'mgarcia', N'hashed_password', N'Maria', N'Garcia', N'Lopez', 2, 1, 1, GETUTCDATE(), NULL),
    (N'jrodriguez', N'hashed_password', N'Juan', N'Rodriguez', N'Hernandez', 1, 2, 1, GETUTCDATE(), NULL),
    (N'alopez', N'hashed_password', N'Ana', N'Lopez', N'Martinez', 3, 1, 2, GETUTCDATE(), NULL);

-- 6. LOGINS (datos de seguimiento de sesiones)
INSERT INTO dbo.Logins
(UserId, Extension, MovementType, Date)
VALUES
    (1, 101, 1, DATEADD(HOUR, -2, GETUTCDATE())),
    (1, 101, 0, GETUTCDATE()),
    (2, 102, 1, DATEADD(HOUR, -1, GETUTCDATE())), 
    (2, 102, 0, GETUTCDATE()),
    (3, 103, 1, GETUTCDATE()),
    (4, 104, 1, DATEADD(MINUTE, -30, GETUTCDATE())); 
