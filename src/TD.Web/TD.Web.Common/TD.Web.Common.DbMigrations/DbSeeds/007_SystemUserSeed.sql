
insert into "Users" ("Id","CityId","Mobile","Type","Address","Username","Password","Nickname","FavoriteStoreId","DateOfBirth","IsActive","CreatedAt","CreatedBy") 
	values (0,(SELECT "Id" FROM "Cities" LIMIT 1),'+000000000000',0,'Unknown','SYSTEM','SomeRandomPassword','SYSTEM', (SELECT "Id" FROM "Stores" LIMIT 1), '1900-01-01 10:00:00.000', true, '2024-01-09 21:26:52.978',0);
