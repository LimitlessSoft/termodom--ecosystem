DO $$ 
DECLARE
    var1 INT;
    var2 INT;
    var3 INT;
    var4 INT;
    var5 INT;
    var6 INT;
    var7 INT;
    var8 INT;
    var9 INT;
    var10 INT;
    var11 INT;
    var12 INT;
BEGIN
    insert into "ProductGroups" ("Name", "ParentGroupId", "IsActive", "CreatedBy", "CreatedAt", "UpdatedBy", "UpdatedAt") values ('Fasade', null, true, 0, current_timestamp, null, null)
    returning "Id" into var1;
    insert into "ProductGroups" ("Name", "ParentGroupId", "IsActive", "CreatedBy", "CreatedAt", "UpdatedBy", "UpdatedAt") values ('Izolacija', null, true, 0, current_timestamp, null, null)
    returning "Id" into var2;
    insert into "ProductGroups" ("Name", "ParentGroupId", "IsActive", "CreatedBy", "CreatedAt", "UpdatedBy", "UpdatedAt") values ('Suva gradnja', null, true, 0, current_timestamp, null, null)
    returning "Id" into var3;
    insert into "ProductGroups" ("Name", "ParentGroupId", "IsActive", "CreatedBy", "CreatedAt", "UpdatedBy", "UpdatedAt") values ('Ogrev', null, true, 0, current_timestamp, null, null)
    returning "Id" into var4;
    insert into "ProductGroups" ("Name", "ParentGroupId", "IsActive", "CreatedBy", "CreatedAt", "UpdatedBy", "UpdatedAt") values ('Ostalo', null, true, 0, current_timestamp, null, null)
    returning "Id" into var5;
    insert into "ProductGroups" ("Name", "ParentGroupId", "IsActive", "CreatedBy", "CreatedAt", "UpdatedBy", "UpdatedAt") values ('Alat', null, true, 0, current_timestamp, null, null)
    returning "Id" into var6;
    insert into "ProductGroups" ("Name", "ParentGroupId", "IsActive", "CreatedBy", "CreatedAt", "UpdatedBy", "UpdatedAt") values ('Malterisanje', null, true, 0, current_timestamp, null, null)
    returning "Id" into var7;
    insert into "ProductGroups" ("Name", "ParentGroupId", "IsActive", "CreatedBy", "CreatedAt", "UpdatedBy", "UpdatedAt") values ('Bašta', null, true, 0, current_timestamp, null, null)
    returning "Id" into var8;
    insert into "ProductGroups" ("Name", "ParentGroupId", "IsActive", "CreatedBy", "CreatedAt", "UpdatedBy", "UpdatedAt") values ('Krov', null, true, 0, current_timestamp, null, null)
    returning "Id" into var9;
    insert into "ProductGroups" ("Name", "ParentGroupId", "IsActive", "CreatedBy", "CreatedAt", "UpdatedBy", "UpdatedAt") values ('Grubi', null, true, 0, current_timestamp, null, null)
    returning "Id" into var10;
    insert into "ProductGroups" ("Name", "ParentGroupId", "IsActive", "CreatedBy", "CreatedAt", "UpdatedBy", "UpdatedAt") values ('Keramika', null, true, 0, current_timestamp, null, null)
    returning "Id" into var11;
    insert into "ProductGroups" ("Name", "ParentGroupId", "IsActive", "CreatedBy", "CreatedAt", "UpdatedBy", "UpdatedAt") values ('Moleraj', null, true, 0, current_timestamp, null, null)
    returning "Id" into var12;
    insert into "ProductGroups" ("Name", "ParentGroupId", "IsActive", "CreatedBy", "CreatedAt", "UpdatedBy", "UpdatedAt") values ('Stiropor i stirodur', @var1, true, 0, current_timestamp, null, null);
    insert into "ProductGroups" ("Name", "ParentGroupId", "IsActive", "CreatedBy", "CreatedAt", "UpdatedBy", "UpdatedAt") values ('Fasadni lepkovi', @var1, true, 0, current_timestamp, null, null);
    insert into "ProductGroups" ("Name", "ParentGroupId", "IsActive", "CreatedBy", "CreatedAt", "UpdatedBy", "UpdatedAt") values ('Kamena vuna', @var1, true, 0, current_timestamp, null, null);
    insert into "ProductGroups" ("Name", "ParentGroupId", "IsActive", "CreatedBy", "CreatedAt", "UpdatedBy", "UpdatedAt") values ('Prateći proizvodi za fasadu', @var1, true, 0, current_timestamp, null, null);
    insert into "ProductGroups" ("Name", "ParentGroupId", "IsActive", "CreatedBy", "CreatedAt", "UpdatedBy", "UpdatedAt") values ('Bavalit', @var1, true, 0, current_timestamp, null, null);
    insert into "ProductGroups" ("Name", "ParentGroupId", "IsActive", "CreatedBy", "CreatedAt", "UpdatedBy", "UpdatedAt") values ('Termoizolacija', @var2, true, 0, current_timestamp, null, null);
    insert into "ProductGroups" ("Name", "ParentGroupId", "IsActive", "CreatedBy", "CreatedAt", "UpdatedBy", "UpdatedAt") values ('Hidroizolacija', @var2, true, 0, current_timestamp, null, null);
    insert into "ProductGroups" ("Name", "ParentGroupId", "IsActive", "CreatedBy", "CreatedAt", "UpdatedBy", "UpdatedAt") values ('Zvučna izolacija', @var2, true, 0, current_timestamp, null, null);
    insert into "ProductGroups" ("Name", "ParentGroupId", "IsActive", "CreatedBy", "CreatedAt", "UpdatedBy", "UpdatedAt") values ('Izolacija za cevi', @var2, true, 0, current_timestamp, null, null);
    insert into "ProductGroups" ("Name", "ParentGroupId", "IsActive", "CreatedBy", "CreatedAt", "UpdatedBy", "UpdatedAt") values ('Gips karton ploče', @var3, true, 0, current_timestamp, null, null);
    insert into "ProductGroups" ("Name", "ParentGroupId", "IsActive", "CreatedBy", "CreatedAt", "UpdatedBy", "UpdatedAt") values ('Profili', @var3, true, 0, current_timestamp, null, null);
    insert into "ProductGroups" ("Name", "ParentGroupId", "IsActive", "CreatedBy", "CreatedAt", "UpdatedBy", "UpdatedAt") values ('Šrafovska roba', @var3, true, 0, current_timestamp, null, null);
    insert into "ProductGroups" ("Name", "ParentGroupId", "IsActive", "CreatedBy", "CreatedAt", "UpdatedBy", "UpdatedAt") values ('Revizioni otvori', @var3, true, 0, current_timestamp, null, null);
    insert into "ProductGroups" ("Name", "ParentGroupId", "IsActive", "CreatedBy", "CreatedAt", "UpdatedBy", "UpdatedAt") values ('Prateći proizvodi za suvu gradnju', @var3, true, 0, current_timestamp, null, null);
    insert into "ProductGroups" ("Name", "ParentGroupId", "IsActive", "CreatedBy", "CreatedAt", "UpdatedBy", "UpdatedAt") values ('Pričvrsni elementi za suvu gradnju', @var3, true, 0, current_timestamp, null, null);
    insert into "ProductGroups" ("Name", "ParentGroupId", "IsActive", "CreatedBy", "CreatedAt", "UpdatedBy", "UpdatedAt") values ('Prirodni ogrev', @var4, true, 0, current_timestamp, null, null);
    insert into "ProductGroups" ("Name", "ParentGroupId", "IsActive", "CreatedBy", "CreatedAt", "UpdatedBy", "UpdatedAt") values ('Ambalaža', @var5, true, 0, current_timestamp, null, null);
    insert into "ProductGroups" ("Name", "ParentGroupId", "IsActive", "CreatedBy", "CreatedAt", "UpdatedBy", "UpdatedAt") values ('Zaštita', @var5, true, 0, current_timestamp, null, null);
    insert into "ProductGroups" ("Name", "ParentGroupId", "IsActive", "CreatedBy", "CreatedAt", "UpdatedBy", "UpdatedAt") values ('Električni alat', @var6, true, 0, current_timestamp, null, null);
    insert into "ProductGroups" ("Name", "ParentGroupId", "IsActive", "CreatedBy", "CreatedAt", "UpdatedBy", "UpdatedAt") values ('Ručni alat', @var6, true, 0, current_timestamp, null, null);
    insert into "ProductGroups" ("Name", "ParentGroupId", "IsActive", "CreatedBy", "CreatedAt", "UpdatedBy", "UpdatedAt") values ('OSB', @var3, true, 0, current_timestamp, null, null);
    insert into "ProductGroups" ("Name", "ParentGroupId", "IsActive", "CreatedBy", "CreatedAt", "UpdatedBy", "UpdatedAt") values ('AMF / Armstrong', @var3, true, 0, current_timestamp, null, null);
    insert into "ProductGroups" ("Name", "ParentGroupId", "IsActive", "CreatedBy", "CreatedAt", "UpdatedBy", "UpdatedAt") values ('Malteri', @var7, true, 0, current_timestamp, null, null);
    insert into "ProductGroups" ("Name", "ParentGroupId", "IsActive", "CreatedBy", "CreatedAt", "UpdatedBy", "UpdatedAt") values ('Lajsne za malterisanje', @var7, true, 0, current_timestamp, null, null);
    insert into "ProductGroups" ("Name", "ParentGroupId", "IsActive", "CreatedBy", "CreatedAt", "UpdatedBy", "UpdatedAt") values ('Dodaci za malterisanje', @var7, true, 0, current_timestamp, null, null);
    insert into "ProductGroups" ("Name", "ParentGroupId", "IsActive", "CreatedBy", "CreatedAt", "UpdatedBy", "UpdatedAt") values ('Kosilice', @var8, true, 0, current_timestamp, null, null);
    insert into "ProductGroups" ("Name", "ParentGroupId", "IsActive", "CreatedBy", "CreatedAt", "UpdatedBy", "UpdatedAt") values ('Krovni pokrivaci', @var9, true, 0, current_timestamp, null, null);
    insert into "ProductGroups" ("Name", "ParentGroupId", "IsActive", "CreatedBy", "CreatedAt", "UpdatedBy", "UpdatedAt") values ('Krovna konstrukcija', @var9, true, 0, current_timestamp, null, null);
    insert into "ProductGroups" ("Name", "ParentGroupId", "IsActive", "CreatedBy", "CreatedAt", "UpdatedBy", "UpdatedAt") values ('Dodaci za krov', @var9, true, 0, current_timestamp, null, null);
    insert into "ProductGroups" ("Name", "ParentGroupId", "IsActive", "CreatedBy", "CreatedAt", "UpdatedBy", "UpdatedAt") values ('Lepkovi za keramiku', @var11, true, 0, current_timestamp, null, null);
    insert into "ProductGroups" ("Name", "ParentGroupId", "IsActive", "CreatedBy", "CreatedAt", "UpdatedBy", "UpdatedAt") values ('Lajsne za keramiku', @var11, true, 0, current_timestamp, null, null);
    insert into "ProductGroups" ("Name", "ParentGroupId", "IsActive", "CreatedBy", "CreatedAt", "UpdatedBy", "UpdatedAt") values ('Ostali proizvodi za keramiku', @var11, true, 0, current_timestamp, null, null);
    insert into "ProductGroups" ("Name", "ParentGroupId", "IsActive", "CreatedBy", "CreatedAt", "UpdatedBy", "UpdatedAt") values ('Boje za krečenje', @var12, true, 0, current_timestamp, null, null);
END $$;