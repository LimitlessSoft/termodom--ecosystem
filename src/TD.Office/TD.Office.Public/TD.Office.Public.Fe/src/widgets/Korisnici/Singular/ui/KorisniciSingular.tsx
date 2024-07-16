import { IKorisniciSingularProps } from '../interfaces/IKorisniciSingularProps'
import { KorisniciSingularDataField } from './KorisniciSingularDataField'
import { officeApi } from '@/apis/officeApi'
import { toast } from 'react-toastify'
import { Grid } from '@mui/material'

export const KorisniciSingular = (props: IKorisniciSingularProps) => {
    return (
        <Grid container p={2} maxWidth={500}>
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
                            .catch((_) => {
                                reject()
                            })
                    })
                }
            />
        </Grid>
    )
}
