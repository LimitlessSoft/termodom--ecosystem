import { KorisniciSingularDataField } from './KorisniciSingularDataField'
import { Grid } from '@mui/material'
import { KorisniciSingularPermissions } from './KorisniciSingularPermissions'
import { KorisniciSingularTipKorisnika } from './KorisniciSingularTipKorisnika'
import korisniciSingularFieldsConfig from '@/data/korisniciSingularFieldsConfig.json'

export const KorisniciSingular = ({ user, onSaveUserData, onUpdateTipKorisnika }) => (
    <Grid container p={2} maxWidth={520} gap={2}>
        {Object.entries(korisniciSingularFieldsConfig.FIELDS).map(
            ([fieldKey, { KEY, LABEL, TYPE, VALIDATION, EDITABLE }]) => {
                const isIntegerValidation = VALIDATION === 'integer'

                return (
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
                                          isIntegerValidation ? +value : value,
                                          {
                                              [KEY]: isIntegerValidation
                                                  ? +value
                                                  : value,
                                          },
                                          LABEL
                                      )
                                : undefined
                        }
                    />
                )
            }
        )}
        <Grid item xs={12}>
            <KorisniciSingularTipKorisnika
                userId={user.id}
                currentTipKorisnikaId={user.tipKorisnikaId}
                onUpdate={onUpdateTipKorisnika}
            />
        </Grid>
        <KorisniciSingularPermissions userId={user.id} />
    </Grid>
)
