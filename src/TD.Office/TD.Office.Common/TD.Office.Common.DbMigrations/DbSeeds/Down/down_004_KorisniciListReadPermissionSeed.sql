-- Remove KorisniciListRead (52) permissions that were seeded
DELETE FROM "UserPermissions" WHERE "Permission" = 52;
