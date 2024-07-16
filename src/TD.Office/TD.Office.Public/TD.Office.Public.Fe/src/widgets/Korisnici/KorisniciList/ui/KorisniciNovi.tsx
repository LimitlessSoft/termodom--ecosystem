import { Accordion, AccordionDetails, AccordionSummary, Button, Grid, Paper, TextField } from "@mui/material"
import { ArrowDownwardRounded } from "@mui/icons-material"
import { toast } from "react-toastify"
import { useState } from "react"
import {ApiBase, ContentType, fetchApi} from "@/api";

export const KorisniciNovi = (): JSX.Element => {

    const [isUpdating, setIsUpdating] = useState<boolean>(false)
    const [request, setRequest] = useState<any>({})

    return (
        <Accordion component={Paper} sx={{
            my: 2,
            maxWidth: 500
        }}>
            <AccordionSummary expandIcon={<ArrowDownwardRounded />}>
                Novi korisnik
            </AccordionSummary>
            <AccordionDetails>
                <Grid container spacing={1}>
                    <Grid item sm={12}>
                        <TextField disabled={isUpdating} label={`Korisničko ime (username)`} fullWidth onChange={(e) => {
                            setRequest((prev: any) => ({ ...prev, username: e.target.value }))
                        }} />
                    </Grid>
                    <Grid item sm={12}>
                        <TextField disabled={isUpdating} label={`Nadimak`} fullWidth onChange={(e) => {
                            setRequest((prev: any) => ({ ...prev, nickname: e.target.value }))
                        }} />
                    </Grid>
                    <Grid item sm={12}>
                        <TextField disabled={isUpdating} label={`Šifra`} fullWidth onChange={(e) => {
                            setRequest((prev: any) => ({ ...prev, password: e.target.value }))
                        }}/>
                    </Grid>
                    <Grid item sm={12}>
                        <Button disabled={isUpdating} variant={`contained`} onClick={() => {
                            setIsUpdating(true)
                            fetchApi(ApiBase.Main, `/users`, {
                                method: `POST`,
                                body: request,
                                contentType: ContentType.ApplicationJson
                            })
                            .then(_ => {
                                toast.success(`Korisnik je uspešno kreiran`)
                            })
                            .finally(() => setIsUpdating(false))
                        }}>Kreiraj korisnika</Button>
                    </Grid>
                </Grid>
            </AccordionDetails>
        </Accordion>
    )
}