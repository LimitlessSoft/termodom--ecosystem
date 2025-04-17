import { KorisniciSingularDataField } from './KorisniciSingularDataField'
import { Grid } from '@mui/material'
import { KorisniciSingularPermissions } from './KorisniciSingularPermissions'
import { USERS_CONSTANTS } from '@/constants'

export const KorisniciSingular = ({ user, onSaveUserData }) => {
    return (
        <Grid container p={2} maxWidth={520} gap={2}>
            {Object.values(
                USERS_CONSTANTS.SINGLE_USER_DATA_FIELDS.UNEDITABLE
            ).map(({ KEY, LABEL }) => (
                <KorisniciSingularDataField
                    key={KEY}
                    defaultValue={user[KEY]}
                    preLabel={`${LABEL}:`}
                />
            ))}
            {Object.entries(
                USERS_CONSTANTS.SINGLE_USER_DATA_FIELDS.EDITABLE
            ).map(([FIELD_KEY, { KEY, LABEL }]) => (
                <KorisniciSingularDataField
                    key={KEY}
                    editable
                    defaultValue={user[KEY] || ''}
                    preLabel={`${LABEL}:`}
                    onSave={(value) =>
                        onSaveUserData(
                            FIELD_KEY,
                            value,
                            { [KEY]: value },
                            LABEL
                        )
                    }
                />
            ))}
            <KorisniciSingularPermissions userId={user.id} />
        </Grid>
    )
}
