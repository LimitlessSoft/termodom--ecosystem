-- Updates ProductGroups table, set Src as formatted Name
UPDATE "ProductGroups"
SET "Src" = LOWER(
        TRANSLATE(
                REPLACE("Name", ' ', '-'),
                'áéíóúÁÉÍÓÚñÑüÜćčđšžĆČĐŠŽ',
                'aeiouAEIOUnNuUccdsszCCDSSZ'
        )
);