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

UPDATE "Users"
SET "ReferentId" = (
    SELECT u."Id" 
    FROM "Users" AS u
    FULL JOIN "old_users" AS u2 ON u.Username = u2.ime
    WHERE u."Username" = (
        SELECT u2.ime
        FROM "old_users" AS u2
        WHERE u2.id = (
            SELECT u3.id
            FROM "old_users" AS u3
            WHERE u2.referent=u3.id
        )
    )
)
WHERE "Username" IN (SELECT "ime" FROM "old_users");
