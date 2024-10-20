import {
    Autocomplete,
    Box,
    CircularProgress,
    Stack,
    TextField,
} from '@mui/material'
import { useZMagacini } from '../../../../zStore'
import { useState } from 'react'
import { toast } from 'react-toastify'
import { DatePicker } from '@mui/x-date-pickers'
import dayjs from 'dayjs'

export const ProracunFilters = () => {
    const magacini = useZMagacini()

    const [odDatuma, setOdDatuma] = useState(new Date())
    const [doDatuma, setDoDatuma] = useState(new Date())

    return (
        <Box>
            {magacini === undefined ? (
                <CircularProgress />
            ) : (
                <Autocomplete
                    sx={{
                        mx: 2,
                        maxWidth: 500,
                    }}
                    defaultValue={magacini[0]}
                    options={magacini}
                    onChange={(event, value) => {
                        toast.error('Magacin promenjen')
                    }}
                    getOptionLabel={(option) => {
                        return `${option.name}`
                    }}
                    renderInput={(params) => <TextField {...params} />}
                />
            )}
            <Stack direction={`row`} m={2} gap={2}>
                <DatePicker
                    label={`Od datuma`}
                    value={dayjs(odDatuma)}
                    onChange={(e) => {
                        setOdDatuma(dayjs(e ?? new Date()).toDate())
                    }}
                />
                <DatePicker
                    label={'Do datuma'}
                    value={dayjs(doDatuma)}
                    onChange={(e) => {
                        setDoDatuma(dayjs(e ?? new Date()).toDate())
                    }}
                />
            </Stack>
        </Box>
    )
}
