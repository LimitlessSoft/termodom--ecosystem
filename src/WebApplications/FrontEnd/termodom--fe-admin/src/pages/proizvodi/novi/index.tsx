import { ApiBase, fetchApi } from "@/app/api"
import { Box, MenuItem, TextField } from "@mui/material"
import { useEffect, useState } from "react"

const textFieldVariant = 'standard'

const ProizvodiNovi = (): JSX.Element => {

    const [units, setUnits] = useState<any>([])

    useEffect(() => {
        fetchApi(ApiBase.Main, "/units").then((response) => {
            setUnits(response.payload)
        })
    }, [])


    return (
        <Box
        sx={{ m: 2, '& .MuiTextField-root': { m: 1, width: '25ch' }, }}>
            <TextField
            required
            id='name'
            label='Ime proizvoda'
            variant={textFieldVariant} />

            <TextField
            required
            id='src'
            label='link (src)'
            variant={textFieldVariant} />

            <TextField
            required
            id='catalogId'
            label='Kataloški broj'
            variant={textFieldVariant} />

            <TextField
            id='unit'
            select
            required
            label='Jedinica mere'
            helperText='Izaberite jedinicu mere proizvoda'>
                {units?.map((unit:any, index:any) => {
                    return (
                        <MenuItem key={`f123sfa${index}`} value={unit.id}>
                            {unit.name}
                        </MenuItem>
                    )
                })}
            </TextField>

            <TextField
            id='classification'
            select
            required
            label='Klasifikacija'
            helperText='Izaberite klasu proizvoda'>
                <MenuItem value={0}>
                    Hobi
                </MenuItem>
                <MenuItem value={1}>
                    Standard
                </MenuItem>
                <MenuItem value={2}>
                    Profi
                </MenuItem>
            </TextField>

            <TextField
            required
            id='vat'
            label='PDV'
            variant={textFieldVariant} />

            grupe

            productpricegrupe
        </Box>
    )
}

export default ProizvodiNovi