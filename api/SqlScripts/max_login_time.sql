-- Usuario con mayor tiempo total logueado

WITH Ordered AS (
    SELECT
        l.UserId,
        l.MovementType,
        l.Date,
        LEAD(l.Date) OVER (
            PARTITION BY l.UserId
            ORDER BY l.Date, l.Id   -- orden estable
            ) AS next_date,
        LEAD(l.MovementType) OVER (
            PARTITION BY l.UserId
            ORDER BY l.Date, l.Id
            ) AS next_type
    FROM Logins l
),
     Totals AS (
         SELECT
             UserId,
             SUM(DATEDIFF(SECOND, Date, next_date)) AS total_seconds
         FROM Ordered
         WHERE MovementType = 1
           AND next_type = 0
           AND next_date > Date   -- evita errores
         GROUP BY UserId
     )
SELECT TOP (1)
    t.UserId AS User_id,
    CONCAT(
            t.total_seconds / 86400, ' días, ',
            (t.total_seconds / 3600) % 24, ' horas, ',
            (t.total_seconds / 60) % 60, ' minutos, ',
            t.total_seconds % 60, ' segundos'
    ) AS [Tiempo total]
FROM Totals t
         JOIN Users u ON u.Id = t.UserId
ORDER BY t.total_seconds DESC;