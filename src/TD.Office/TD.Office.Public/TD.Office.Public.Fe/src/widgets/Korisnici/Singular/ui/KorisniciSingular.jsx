import { KorisniciSingularDataField } from './KorisniciSingularDataField'
import { Grid } from '@mui/material'
import { KorisniciSingularPermissions } from './KorisniciSingularPermissions'
import korisniciSingularFieldsConfig from '@/data/korisniciSingularFieldsConfig.json'

export const KorisniciSingular = ({ user, onSaveUserData }) => (
    <Grid container p={2} maxWidth={520} gap={2}>
        {Object.entries(korisniciSingularFieldsConfig.FIELDS).map(
            ([fieldKey, { KEY, LABEL, TYPE, VALIDATION, EDITABLE }]) => (
                <KorisniciSingularDataField
                    key={fieldKey}
                    editable={EDITABLE}
                    defaultValue={user[KEY] || ''}
                    preLabel={`${LABEL}:`}
                    type={TYPE}
                    validation={VALIDATION}
                    onSave={
                        EDITABLE
                            ? (value) =>
                                  onSaveUserData(
                                      fieldKey,
                                      VALIDATION === 'integer' ? +value : value,
                                      {
                                          [KEY]:
                                              VALIDATION === 'integer'
                                                  ? +value
                                                  : value,
                                      },
                                      LABEL
                                  )
                            : undefined
                    }
                />
            )
        )}
        <KorisniciSingularPermissions userId={user.id} />
    </Grid>
)
