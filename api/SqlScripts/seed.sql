-- 1. CREAR UN ÁREA
INSERT INTO dbo.Areas (Name, Status, CreatedAt)
VALUES (N'Sistemas', 1, GETUTCDATE());

-- 2. CREAR UN USUARIO
INSERT INTO dbo.Users
(Username, Password, FirstName, LastName, SecondLastName, UserTypeId, Status, AreaId, CreatedAt, LastLoginAttempt)
VALUES
    (N'jsmith', N'hashed_password', N'John', N'Smith', N'Doe', 1, 1, 1, GETUTCDATE(), NULL);
