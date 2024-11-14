import { KorisniciSingularDataField } from './KorisniciSingularDataField'
import { Grid } from '@mui/material'
import { KorisniciSingularPermissions } from './KorisniciSingularPermissions'

export const KorisniciSingular = ({
    user,
    onSaveUserData,
    FIELD_KEYS,
    FIELD_LABELS,
}) => {
    return (
        <Grid container p={2} maxWidth={500} gap={2}>
            <KorisniciSingularDataField
                defaultValue={user.id}
                preLabel={`${FIELD_LABELS.ID}:`}
            />
            <KorisniciSingularDataField
                defaultValue={user.username}
                preLabel={`${FIELD_LABELS.USERNAME}:`}
            />
            <KorisniciSingularDataField
                editable
                defaultValue={user[FIELD_KEYS.NADIMAK]}
                preLabel={`${FIELD_LABELS.NADIMAK}:`}
                onSave={(value) =>
                    onSaveUserData(
                        FIELD_KEYS.NADIMAK,
                        value,
                        {
                            nickname: value,
                        },
                        FIELD_LABELS.NADIMAK
                    )
                }
            />
            <KorisniciSingularDataField
                editable
                defaultValue={user[FIELD_KEYS.MAX_RABAT_MP_DOKUMENTI]}
                preLabel={`${FIELD_LABELS.MAX_RABAT_MP_DOKUMENTI}:`}
                onSave={(value) =>
                    onSaveUserData(
                        FIELD_KEYS.MAX_RABAT_MP_DOKUMENTI,
                        value,
                        {
                            MaxRabatMPDokumenti: value,
                        },
                        FIELD_LABELS.MAX_RABAT_MP_DOKUMENTI
                    )
                }
            />
            <KorisniciSingularDataField
                editable
                defaultValue={user[FIELD_KEYS.MAX_RABAT_VP_DOKUMENTI]}
                preLabel={`${FIELD_LABELS.MAX_RABAT_VP_DOKUMENTI}:`}
                onSave={(value) =>
                    onSaveUserData(
                        FIELD_KEYS.MAX_RABAT_VP_DOKUMENTI,
                        value,
                        {
                            MaxRabatVPDokumenti: value,
                        },
                        FIELD_LABELS.MAX_RABAT_VP_DOKUMENTI
                    )
                }
            />
            <KorisniciSingularPermissions userId={user.id} />
        </Grid>
    )
}
