import { ApiBase, fetchApi } from '@/app/api'
import { KontaktTitle } from '@/app/constants'
import { CustomHead } from '@/widgets/CustomHead'
import { Grid, Stack, Typography } from '@mui/material'
import { useEffect, useState } from 'react'

const Kontakt = (): JSX.Element => {
    const [stores, setStores] = useState<any | null>(null)

    useEffect(() => {
        fetchApi(ApiBase.Main, '/stores?sortColumn=Name').then((r) => {
            r.json().then((r: any) => setStores(r))
        })
    }, [])

    return (
        <Grid
            container
            sx={{
                justifyContent: `center`,
                px: 1,
            }}
        >
            <CustomHead title={KontaktTitle} />
            <Grid item sm={12}>
                <Stack
                    spacing={1}
                    sx={{
                        textAlign: `center`,
                        py: 10,
                    }}
                >
                    <Typography
                        component={`h1`}
                        sx={{
                            py: 2,
                        }}
                        variant={`h4`}
                    >
                        Termodom <br /> Radno vreme stovarišta
                    </Typography>
                    <Typography>Ponedeljak - Petak: 07:30 - 15:30</Typography>
                    <Typography>Subota: 07:30 - 14:30</Typography>
                    <Typography>Nedelja: Ne radimo</Typography>
                </Stack>
                <Stack
                    spacing={1}
                    sx={{
                        textAlign: `center`,
                    }}
                >
                    <Typography variant={`h6`}>
                        <b>Web podrška: 064 108 39 32</b>
                    </Typography>
                </Stack>
                <Stack
                    spacing={1}
                    sx={{
                        my: 5,
                        textAlign: `center`,
                    }}
                >
                    <Typography
                        component={`h1`}
                        sx={{
                            py: 2,
                        }}
                        variant={`h4`}
                    >
                        Lokacije
                    </Typography>
                    {stores &&
                        stores.map((store: any, index: number) => {
                            if (store.id == -5) return

                            return (
                                <Stack
                                    key={index}
                                    spacing={1}
                                    sx={{
                                        textAlign: `center`,
                                    }}
                                >
                                    <Typography>{store.name}</Typography>
                                </Stack>
                            )
                        })}
                </Stack>
            </Grid>
        </Grid>
    )
}

export default Kontakt
