
BEGIN TRANSACTION

update Usuarios set password = LOWER(CONVERT(char(100),HASHBYTES('SHA2_256', password),2))

COMMIT
                                 