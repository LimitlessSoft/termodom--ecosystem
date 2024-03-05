DELETE FROM "Users"
WHERE "Username" IN (SELECT "ime" FROM "old_users");

DROP TABLE "old_users";