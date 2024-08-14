import { IKorisniciSingularProps } from '../interfaces/IKorisniciSingularProps'
import { KorisniciSingularDataField } from './KorisniciSingularDataField'
import { handleApiError, officeApi } from '@/apis/officeApi'
import { toast } from 'react-toastify'
import { Grid } from '@mui/material'
import { KorisniciSingularPermissions } from './KorisniciSingularPermissions'

export const KorisniciSingular = (props: IKorisniciSingularProps) => {
    return (
        <Grid container p={2} maxWidth={500} gap={2}>
            <KorisniciSingularDataField
                defaultValue={props.user.id}
                preLabel={`Id:`}
            />
            <KorisniciSingularDataField
                defaultValue={props.user.username}
                preLabel={`Username:`}
            />
            <KorisniciSingularDataField
                editable
                defaultValue={props.user.nickname}
                preLabel={`Nadimak:`}
                onSave={(v) =>
                    new Promise<void>((resolve, reject) => {
                        officeApi
                            .put(`/users/${props.user.id}/nickname`, {
                                id: props.user.id,
                                nickname: v,
                            })
                            .then(() => {
                                toast.success(`Nadimak je uspešno sačuvan`)
                                resolve()
                            })
                            .catch((err) => {
                                reject()
                                handleApiError(err)
                            })
                    })
                }
            />
            <KorisniciSingularPermissions userId={props.user.id} />
        </Grid>
    )
}
