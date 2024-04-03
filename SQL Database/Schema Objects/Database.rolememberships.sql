EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'cableone\mccauleyr';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'cableone\mccauleyr';


GO
EXECUTE sp_addrolemember @rolename = N'db_datawriter', @membername = N'cableone\mccauleyr';

