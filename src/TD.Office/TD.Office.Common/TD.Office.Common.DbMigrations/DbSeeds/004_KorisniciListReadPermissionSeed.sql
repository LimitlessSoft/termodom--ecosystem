-- Seed KorisniciListRead (52) permission for all users who have KorisniciRead (7)
INSERT INTO "UserPermissions" ("Permission", "UserId", "IsActive", "CreatedBy", "CreatedAt")
SELECT 52, "UserId", true, 0, current_timestamp
FROM "UserPermissions"
WHERE "Permission" = 7 AND "IsActive" = true
AND "UserId" NOT IN (
    SELECT "UserId" FROM "UserPermissions" WHERE "Permission" = 52 AND "IsActive" = true
);
