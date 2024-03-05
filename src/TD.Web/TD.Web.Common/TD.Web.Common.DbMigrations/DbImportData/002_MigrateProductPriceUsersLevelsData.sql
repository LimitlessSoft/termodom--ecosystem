INSERT INTO "ProductPriceGroupLevel" ("UserId", "Level", "ProductPriceGroupId", "IsActive", "CreatedAt", "CreatedBy")
   select
   u."Id",
   ouc.nivo,
   case when (ouc.cenovnik_grupaid + 1) > 8 THEN 7 ELSE (ouc.cenovnik_grupaid + 1) end as "GrupaId",
   true,
   current_timestamp,
   0
       from "old_user_cenovnik" ouc
       left join "old_users" ou on ouc.userid = ou.id
       left join "Users" u on ou.ime = u."Username"
       where u."Id" is not null