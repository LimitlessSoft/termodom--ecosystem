import { handleApiError, webApi } from '@/api/webApi'
import { useUser } from '@/app/hooks'
import { CenteredContentWrapper } from '@/widgets/CenteredContentWrapper'
import { CustomHead } from '@/widgets/CustomHead'
import { ProizvodCard } from '@/widgets/Proizvodi/ProizvodiList/ui/ProizvodCard'
import {
    Button,
    CircularProgress,
    Grid,
    Stack,
    Typography,
} from '@mui/material'
import { useRouter } from 'next/router'
import { useEffect, useState } from 'react'
import { toast } from 'react-toastify'

const ProizvodiOmiljeni = () => {
    const user = useUser(true, true)

    const router = useRouter()
    const [omiljeni, setOmiljeni] = useState<any | undefined>(undefined)
    const [isError, setIsError] = useState<boolean>(false)

    useEffect(() => {
        setIsError(false)
        webApi
            .get('/favorite-products')
            .then((res) => setOmiljeni(res.data.payload))
            .catch((error) => {
                setIsError(true)
                handleApiError(error)
            })
    }, [])

    return (
        <CenteredContentWrapper>
            <CustomHead />
            <Stack p={2} spacing={2} width={`100%`}>
                <Grid>
                    <Button
                        variant={`contained`}
                        onClick={() => {
                            router.push('/')
                        }}
                    >
                        Povratak na sve proizvode
                    </Button>
                </Grid>
                <Grid>
                    <Typography variant={`h5`}>
                        Lista omiljenih proizvoda
                    </Typography>
                </Grid>
                <Grid container>
                    {isError && (
                        <Typography variant={`h6`}>
                            Došlo je do greške prilikom učitavanja omiljenih
                            proizvoda
                        </Typography>
                    )}
                    {!isError && !omiljeni && <CircularProgress />}
                    {!isError && omiljeni && omiljeni.length === 0 && (
                        <Typography variant={`h6`}>
                            Morate obaviti barem jednu kupovinu kako bi
                            analizirali vaše omiljene proizvode
                        </Typography>
                    )}
                    {!isError && omiljeni && omiljeni.length > 0 && (
                        <Grid container justifyContent={'center'}>
                            {omiljeni.map((o: any) => {
                                return (
                                    <ProizvodCard
                                        key={o.id}
                                        proizvod={o}
                                        user={user}
                                    />
                                )
                            })}
                        </Grid>
                    )}
                </Grid>
            </Stack>
        </CenteredContentWrapper>
    )
}

export default ProizvodiOmiljeni
