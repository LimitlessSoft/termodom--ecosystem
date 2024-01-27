import { ApiBase, fetchApi } from "@/app/api"
import { Button, CircularProgress, Grid, MenuItem, Stack, TextField, Typography } from "@mui/material"
import { useEffect, useState } from "react"

const textFieldVariant = 'filled'

export const KorpaZakljucivanje = (): JSX.Element => {

    const [stores, setStores] = useState<any | undefined>(null)

    useEffect(() => {
        fetchApi(ApiBase.Main, `/stores?sortColumn=name`)
        .then((res) => {
            setStores(res)
        })
    }, [])

    return (
        <Grid my={5}>
            <Stack alignItems={`center`} direction={`column`}
            spacing={2}>
                {
                    stores == null ?
                        <CircularProgress /> :
                        <TextField
                            id='mesto-preuzimanja'
                            select
                            required
                            label='Mesto preuzimanja'
                            sx={{ minWidth: 350 }}
                            onChange={(e) => {
                                // setNewUser((prev) => { return { ...prev, favoriteStoreId: Number.parseInt(e.target.value) }})
                            }}
                            helperText='Izaberite mesto preuzimanja'>
                                {
                                    stores.map((store: any) => {
                                        return (
                                            <MenuItem key={store.id} value={store.id}>
                                                {store.name}
                                            </MenuItem>
                                        )
                                    })
                                }
                        </TextField>
                }

                <TextField
                    required
                    sx={{ m: 1, minWidth: 350 }}
                    id='ime-i-prezime'
                    label='Ime i prezime'
                    onChange={(e) => {
                        // setLoginRequest((prev) => { return { ...prev, username: e.target.value }})
                    }}
                    variant={textFieldVariant} />

                <TextField
                    required
                    sx={{ m: 1, minWidth: 350 }}
                    id='mobilni'
                    label='Mobilni telefon'
                    onChange={(e) => {
                        // setLoginRequest((prev) => { return { ...prev, username: e.target.value }})
                    }}
                    variant={textFieldVariant} />

                <TextField
                    sx={{ m: 1, minWidth: 350 }}
                    id='napomena'
                    label='Napomena'
                    onChange={(e) => {
                        // setLoginRequest((prev) => { return { ...prev, username: e.target.value }})
                    }}
                    variant={textFieldVariant} />

                <TextField
                    id='nacini-placanja'
                    select
                    required
                    label='Način plaćanja'
                    sx={{ minWidth: 350 }}
                    onChange={(e) => {
                        // setNewUser((prev) => { return { ...prev, favoriteStoreId: Number.parseInt(e.target.value) }})
                    }}
                    helperText='Izaberite način plaćanja'>
                        <MenuItem value={1}>
                            Gotovinom
                        </MenuItem>
                        <MenuItem value={2}>
                            Virmanom
                        </MenuItem>
                        {/* {
                            stores.map((store: any) => {
                                return (
                                    <MenuItem key={store.id} value={store.id}>
                                        {store.name}
                                    </MenuItem>
                                )
                            })
                        } */}
                </TextField>

                <Typography>
                    Cene iz ove porudžbine važe 1 dan/a od dana zaključivanja!
                </Typography>

                <Button
                    variant={`contained`}>
                    Zaključi porudžbinu
                </Button>
            </Stack>
        </Grid>
    )
}