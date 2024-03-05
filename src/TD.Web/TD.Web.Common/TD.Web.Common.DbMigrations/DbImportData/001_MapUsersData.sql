INSERT INTO "Users" ("Username", "Mail", "Mobile","ProcessingDate", "Type", "PIB", "PPID", "Address", "Password", "Nickname", "DateOfBirth", "IsActive", "CreatedAt", "CreatedBy", "Comment", "FavoriteStoreId", "ProfessionId", "CityId")
SELECT 
    ou.ime, 
    ou.mail, 
    ou.mobilni,
    ou.datum_odobrenja,
    ou.tip,
    ou.pib, 
    ou.ppid, 
    ou.adresa_stanovanja, 
    ou.pw, 
    COALESCE(ou.nadimak, ou.ime) AS "Nickname", 
    ou.datum_rodjenja, 
    CASE 
        WHEN ou.datum_odobrenja IS NOT NULL THEN true 
        ELSE false 
    END AS "IsActive",
    current_timestamp, 
    0, 
    ou.komentar, 
    COALESCE(
        (SELECT s."Id" FROM "Stores" s WHERE s."Id" = ou.magacinid + 100), 
        -5
    ) AS "FavoriteStoreId",
    p."Id", 
    0
FROM 
    old_users ou
JOIN 
    "Professions" p ON p."Name" = 'Građanin';

UPDATE "Users" users
SET "ReferentId" = (
    select u1."Id" from "Users" u
        left join old_users ou on ou.ime = u."Username"
        left join old_users ou2 on ou.referent = ou2.id
        left join "Users" u1 on ou2.ime = u1."Username"
    where u."Id" = users."Id"
)
