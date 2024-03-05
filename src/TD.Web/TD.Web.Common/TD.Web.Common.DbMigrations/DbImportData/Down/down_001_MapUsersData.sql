DELETE FROM "Users" WHERE "Username"=(SELECT "ime" FROM "old_users");
DROP TABLE "old_users"