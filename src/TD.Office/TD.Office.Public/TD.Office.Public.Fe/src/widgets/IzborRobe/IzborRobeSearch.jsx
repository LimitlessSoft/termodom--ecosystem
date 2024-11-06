import { Stack, TextField } from '@mui/material'
import { useEffect, useState } from 'react'

export const IzborRobeSearch = (props) => {
    const [search, setSearch] = useState('')

    useEffect(() => {
        props.onChange(search)
    }, [props, search])

    return (
        <Stack direction={`row`}>
            <TextField
                label={`Pretraga`}
                value={search}
                onChange={(e) => {
                    setSearch(e.target.value)
                }}
            />
        </Stack>
    )
}
