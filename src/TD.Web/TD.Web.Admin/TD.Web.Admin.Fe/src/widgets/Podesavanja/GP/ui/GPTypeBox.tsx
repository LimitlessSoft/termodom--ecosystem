import { Grid, MenuItem, TextField } from '@mui/material'
import { adminApi, handleApiError } from '@/apis/adminApi'
import { toast } from 'react-toastify'
import { useState } from 'react'

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

                        adminApi
                            .put(
                                `/products-groups/${props.params.row.id}/type`,
                                {
                                    type: e.target.value,
                                }
                            )
                            .then(() => {
                                toast.success('Grupa uspešno ažurirana!')
                            })
                            .catch((err) => handleApiError(err))
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
