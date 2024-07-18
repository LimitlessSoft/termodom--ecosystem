import { ApiBase, ContentType, fetchApi } from '@/api'
import { Grid, MenuItem, TextField } from '@mui/material'
import { useState } from 'react'
import { toast } from 'react-toastify'

export const GPTypeBox = (props: any): JSX.Element => {
    const [isUpdating, setIsUpdating] = useState<boolean>(false)

    return (
        <Grid container justifyContent={`center`}>
            <Grid item>
                <TextField
                    select
                    defaultValue={props.params.row.typeId}
                    disabled={isUpdating}
                    onChange={(e) => {
                        setIsUpdating(true)
                        fetchApi(
                            ApiBase.Main,
                            `/products-groups/${props.params.row.id}/type`,
                            {
                                method: 'PUT',
                                body: {
                                    type: e.target.value,
                                },
                                contentType: ContentType.ApplicationJson,
                            }
                        )
                            .then(() => {
                                toast.success('Grupa uspešno ažurirana!')
                            })
                            .finally(() => {
                                setIsUpdating(false)
                            })
                    }}
                >
                    {props.productGroupTypes.map((option: any) => (
                        <MenuItem key={option.id} value={option.id}>
                            {option.name}
                        </MenuItem>
                    ))}
                </TextField>
            </Grid>
        </Grid>
    )
}
