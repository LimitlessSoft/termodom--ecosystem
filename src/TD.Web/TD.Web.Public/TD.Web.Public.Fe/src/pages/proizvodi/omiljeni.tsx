import { ApiBase, fetchApi } from "@/app/api";
import { useUser } from "@/app/hooks";
import { CenteredContentWrapper } from "@/widgets/CenteredContentWrapper";
import { CustomHead } from "@/widgets/CustomHead";
import { ProizvodCard } from "@/widgets/Proizvodi/ProizvodiList/ui/ProizvodCard";
import { Button, CircularProgress, Grid, Stack, Typography } from "@mui/material"
import { useRouter } from "next/router"
import { useEffect, useState } from "react";
import { toast } from "react-toastify";

const Omiljeni = (): JSX.Element => {
    
    const user = useUser(true, true)

    const router = useRouter()
    const [omiljeni, setOmiljeni] = useState<any | undefined>(undefined)
    const [isError, setIsError] = useState<boolean>(false)

    useEffect(() => {
        setIsError(false)
        fetchApi(ApiBase.Main, `/favorite-products`)
            .then((payload) => {
                setOmiljeni(payload)
            })
            .catch((error) => {
                setIsError(true)
                toast.error("Došlo je do greške prilikom učitavanja omiljenih proizvoda")
            })
    }, [])

    return (
        <CenteredContentWrapper>
            <CustomHead/>
            <Stack p={2} spacing={2} width={`100%`}>
                <Grid>
                    <Button variant={`contained`} onClick={() => {
                        router.push("/proizvodi")
                    }}>
                        Povratak na sve proizvode
                    </Button>
                </Grid>
                <Grid>
                    <Typography variant={`h5`}>
                        Lista omiljenih proizvoda
                    </Typography>
                </Grid>
                <Grid container>
                    { isError && <Typography variant={`h6`}>Došlo je do greške prilikom učitavanja omiljenih proizvoda</Typography> }
                    { !isError && omiljeni === undefined && <CircularProgress /> }
                    { !isError && omiljeni !== undefined && omiljeni.length === 0 && <Typography variant={`h6`}>Morate obaviti barem jednu kupovinu kako bi analizirali vaše omiljene proizvode</Typography> }
                    { !isError && omiljeni !== undefined && omiljeni.length > 0 &&
                        <Stack 
                            width={'100%'}
                            direction={'column'}>
                            {
                                omiljeni.map((o: any) => {
                                    return <ProizvodCard key={o.id} proizvod={o} user={user} />
                                })
                            }
                        </Stack>
                    }

                </Grid>
            </Stack>
        </CenteredContentWrapper>
    )
}

export default Omiljeni