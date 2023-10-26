import { fetchApi } from "@/app/api"
import { Box, LinearProgress, Typography } from "@mui/material"
import { useEffect, useState } from "react"

export const CGP = (): JSX.Element => {

    const [cenovneGrupeProizvoda, setCenovneGrupeProizvoda] = useState<any | undefined>(null)

    useEffect(() => {
    }, [])

    return (
        <Box>
            <Box>
                <Typography
                    variant='h6'>
                    Cenovne grupe proizvoda
                </Typography>
            </Box>
            <Box>
                {
                    cenovneGrupeProizvoda == null ?
                    <LinearProgress /> :
                    'products'
                }
            </Box>
        </Box>
    )
}