import { Autocomplete, LinearProgress, TextField } from '@mui/material'
import { useZMagacini } from '../../../zStore'

export const MagaciniDropdown = (props) => {
    const magacini = useZMagacini()

    if (!magacini) return <LinearProgress />

    return (
        <Autocomplete
            sx={{
                width: props.width ?? 300,
            }}
            getOptionLabel={(option) => option.name}
            renderInput={(params) => {
                return <TextField {...params} label={'Magacin'} />
            }}
            options={magacini}
            defaultValue={magacini[0]}
            onChange={(e, value) => {
                props.onChange(value.id)
            }}
        />
    )
}
