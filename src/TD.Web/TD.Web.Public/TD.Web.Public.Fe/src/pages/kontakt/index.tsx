import { handleApiError, webApi } from '@/api/webApi'
import { KontaktTitle } from '@/app/constants'
import { CustomHead } from '@/widgets/CustomHead'
import { Box, Button, Grid, Stack, Typography } from '@mui/material'
import { useEffect, useState } from 'react'
import { Phone } from '@mui/icons-material'
import { mainTheme } from '@/app/theme'
import NextLink from 'next/link'
import { PHONE_CONSTANTS } from '@/constants'
import { isDeliveryPickupPlace } from '@/utils/storeUtils'

const Kontakt = () => {
    const [stores, setStores] = useState<any | null>(null)

    useEffect(() => {
        webApi
            .get('/stores?sortColumn=Name')
            .then((res) => {
                setStores(res.data)
            })
            .catch((err) => handleApiError(err))
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
                    <Box sx={{ textAlign: `center`, py: 2 }}>
                        <Typography component={`h1`} variant={`h3`}>
                            Termodom
                        </Typography>
                        <Typography variant={`h4`}>
                            Radno vreme stovarišta
                        </Typography>
                    </Box>
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
                    <Grid>
                        <Button
                            color={`secondary`}
                            LinkComponent={NextLink}
                            href={`tel:${PHONE_CONSTANTS.SASA_PHONE}`}
                        >
                            <Box
                                sx={{
                                    py: 1,
                                    px: 1.5,
                                    mx: 1,
                                    borderRadius: '50%',
                                    backgroundColor:
                                        mainTheme.palette.secondary.main,
                                }}
                            >
                                <Phone
                                    fontSize={`small`}
                                    sx={{
                                        color: mainTheme.palette.common.white,
                                        transform: 'translateY(2px)',
                                    }}
                                />
                            </Box>
                            <Typography variant={`h5`}>
                                {PHONE_CONSTANTS.SASA_PHONE}
                            </Typography>
                        </Button>
                    </Grid>
                </Stack>
                <Stack
                    spacing={1}
                    sx={{
                        my: 5,
                        textAlign: `center`,
                    }}
                >
                    <Typography
                        sx={{
                            py: 2,
                        }}
                        variant={`h4`}
                    >
                        Lokacije
                    </Typography>
                    {stores &&
                        stores.map((store: any, index: number) => {
                            if (isDeliveryPickupPlace(store.id)) return

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
