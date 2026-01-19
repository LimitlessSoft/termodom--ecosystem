import {
    Accordion,
    AccordionDetails,
    AccordionSummary,
    Button,
    Grid,
    Paper,
    TextField,
    FormControl,
    InputLabel,
    Select,
    MenuItem,
} from '@mui/material'
import { ArrowDownwardRounded } from '@mui/icons-material'
import { toast } from 'react-toastify'
import { useState } from 'react'
import { handleApiError, officeApi } from '@/apis/officeApi'

export const KorisniciNovi = ({ tipoviKorisnika = [] }) => {
    const [isUpdating, setIsUpdating] = useState(false)
    const [request, setRequest] = useState({})

    return (
        <Accordion
            component={Paper}
            sx={{
                my: 2,
                maxWidth: 500,
            }}
        >
            <AccordionSummary expandIcon={<ArrowDownwardRounded />}>
                Novi korisnik
            </AccordionSummary>
            <AccordionDetails>
                <Grid container spacing={1}>
                    <Grid item sm={12}>
                        <TextField
                            disabled={isUpdating}
                            label={`Korisničko ime (username)`}
                            fullWidth
                            onChange={(e) => {
                                setRequest((prev) => ({
                                    ...prev,
                                    username: e.target.value,
                                }))
                            }}
                        />
                    </Grid>
                    <Grid item sm={12}>
                        <TextField
                            disabled={isUpdating}
                            label={`Nadimak`}
                            fullWidth
                            onChange={(e) => {
                                setRequest((prev) => ({
                                    ...prev,
                                    nickname: e.target.value,
                                }))
                            }}
                        />
                    </Grid>
                    <Grid item sm={12}>
                        <TextField
                            disabled={isUpdating}
                            label={`Šifra`}
                            fullWidth
                            onChange={(e) => {
                                setRequest((prev) => ({
                                    ...prev,
                                    password: e.target.value,
                                }))
                            }}
                        />
                    </Grid>
                    <Grid item sm={12}>
                        <FormControl fullWidth disabled={isUpdating}>
                            <InputLabel>Tip korisnika</InputLabel>
                            <Select
                                value={request.tipKorisnikaId || ''}
                                label="Tip korisnika"
                                onChange={(e) => {
                                    setRequest((prev) => ({
                                        ...prev,
                                        tipKorisnikaId: e.target.value || null,
                                    }))
                                }}
                            >
                                <MenuItem value="">Bez tipa</MenuItem>
                                {tipoviKorisnika.map((tip) => (
                                    <MenuItem key={tip.id} value={tip.id}>
                                        {tip.naziv}
                                    </MenuItem>
                                ))}
                            </Select>
                        </FormControl>
                    </Grid>
                    <Grid item sm={12}>
                        <Button
                            disabled={isUpdating}
                            variant={`contained`}
                            onClick={() => {
                                setIsUpdating(true)

                                officeApi
                                    .post(`/users`, request)
                                    .then(() => {
                                        toast.success(
                                            `Korisnik je uspešno kreiran`
                                        )
                                    })
                                    .catch((err) => handleApiError(err))
                                    .finally(() => setIsUpdating(false))
                            }}
                        >
                            Kreiraj korisnika
                        </Button>
                    </Grid>
                </Grid>
            </AccordionDetails>
        </Accordion>
    )
}
