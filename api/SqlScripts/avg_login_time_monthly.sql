-- Promedio de logueo por usuario por mes

WITH Ordered AS (
    SELECT
        l.UserId,
        l.Date,
        l.MovementType,
        LEAD(l.Date) OVER (
            PARTITION BY l.UserId
            ORDER BY l.Date, l.Id
            ) AS next_date,
        LEAD(l.MovementType) OVER (
            PARTITION BY l.UserId
            ORDER BY l.Date, l.Id
            ) AS next_type
    FROM Logins l
),
     Sessions AS (
         SELECT
             UserId,
             Date AS login_time,
             DATEDIFF(SECOND, Date, next_date) AS duration_seconds
         FROM Ordered
         WHERE MovementType = 1
           AND next_type = 0
           AND next_date > Date
     ),
     Averages AS (
         SELECT
             UserId,
             YEAR(login_time) AS year,
             MONTH(login_time) AS month,
             CAST(AVG(duration_seconds) AS BIGINT) AS avg_seconds
         FROM Sessions
         GROUP BY
             UserId,
             YEAR(login_time),
             MONTH(login_time)
     )
SELECT
    CONCAT(
            'Usuario ', a.UserId,
            ' en ', LOWER(DATENAME(MONTH, DATEFROMPARTS(a.year, a.month, 1))),
            ' ', a.year, ': ',
            a.avg_seconds / 86400, ' días, ',
            (a.avg_seconds / 3600) % 24, ' horas, ',
            (a.avg_seconds / 60) % 60, ' minutos, ',
            a.avg_seconds % 60, ' segundos'
    ) AS resultado

FROM Averages a
ORDER BY a.UserId, a.year, a.month;