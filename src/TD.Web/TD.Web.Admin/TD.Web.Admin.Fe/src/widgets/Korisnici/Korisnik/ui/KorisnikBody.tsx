import { Button, CircularProgress, Grid, MenuItem, Stack, TextField, Typography, radioClasses } from "@mui/material"
import moment from "moment"
import { KorisnikBodyInfoDataWrapperStyled } from "./KorisnikBodyInfoDataWrapperStyled"
import { DatePicker } from "@mui/x-date-pickers"
import { mainTheme } from "@/app/theme"

export const KorisnikBody = (props: any): JSX.Element => {
    return (
        props.user === undefined ?
            <CircularProgress /> :
            <Grid container>
                <Grid item sm={4}
                    sx={{
                        padding: 2,
                    }}>
                    <Grid container
                        sx={{
                            padding: 2,
                            borderRadius: 2,
                            backgroundColor: mainTheme.palette.secondary.main,
                            color: mainTheme.palette.secondary.contrastText
                        }}
                        direction={`column`}>
                        <Typography>
                            Datum kreiranja naloga: {moment(props.user.createdAt).format("DD.MM.yyyy")}
                        </Typography>
                        <Typography>
                            Datum obrade: { props.user.processingDate !== null ? moment(props.user.processingDate).format("DD.MM.yyyy") : "Još uvek nije obrađen"}
                        </Typography>
                        <Typography>
                            Poslednji put viđen: { props.user.lastTimeSeen !== null ? moment(props.user.lastTimeSeen).format("DD.MM.yyyy") : "Nikada"}
                        </Typography>
                    </Grid>
                </Grid>
                <Grid item sm={8}
                    sx={{
                        padding: 2
                    }}>
                        <KorisnikBodyInfoDataWrapperStyled>
                            <TextField
                                variant={`filled`}
                                defaultValue={props.user.nickname}
                                label={`Nadimak`} />
                        </KorisnikBodyInfoDataWrapperStyled>
                        <KorisnikBodyInfoDataWrapperStyled>
                            <TextField
                                select
                                variant={`filled`}
                                defaultValue={props.user.profession ?? 1}
                                label={`Zanimanje`}>
                                    <MenuItem value={1}>Programer</MenuItem>
                            </TextField>
                        </KorisnikBodyInfoDataWrapperStyled>
                        <KorisnikBodyInfoDataWrapperStyled>
                            <TextField
                                variant={`filled`}
                                defaultValue={props.user.ppid}
                                label={`PPID`} />
                        </KorisnikBodyInfoDataWrapperStyled>
                        <Stack p={`5px`}>
                            <Typography>
                                Datum rođenja
                            </Typography>
                            <DatePicker
                                sx={{
                                    maxWidth: 200,
                                    width: `100%`
                                }}
                                onChange={(e: any) => {
                                }}/>
                        </Stack>
                        <KorisnikBodyInfoDataWrapperStyled>
                            <TextField
                                select
                                variant={`filled`}
                                defaultValue={props.user.city.id}
                                label={`Mesto`}>
                                    <MenuItem value={53}>Beograd</MenuItem>
                            </TextField>
                        </KorisnikBodyInfoDataWrapperStyled>
                        <KorisnikBodyInfoDataWrapperStyled>
                            <TextField
                                variant={`filled`}
                                defaultValue={props.user.address}
                                label={`Adresa`} />
                        </KorisnikBodyInfoDataWrapperStyled>
                        <KorisnikBodyInfoDataWrapperStyled>
                            <TextField
                                variant={`filled`}
                                defaultValue={props.user.mobile}
                                label={`Mobilni`} />
                        </KorisnikBodyInfoDataWrapperStyled>
                        <KorisnikBodyInfoDataWrapperStyled>
                            <TextField
                                variant={`filled`}
                                defaultValue={props.user.mail}
                                label={`Mail`} />
                        </KorisnikBodyInfoDataWrapperStyled>
                        <KorisnikBodyInfoDataWrapperStyled>
                            <TextField
                                select
                                variant={`filled`}
                                defaultValue={props.user.favoriteStore.id}
                                label={`Omiljena radnja`}>
                                    <MenuItem value={117}>Beograd</MenuItem>
                            </TextField>
                        </KorisnikBodyInfoDataWrapperStyled>
                        <KorisnikBodyInfoDataWrapperStyled>
                            <TextField
                                multiline={true}
                                fullWidth={true}
                                minRows={4}
                                defaultValue={props.user.favoriteStore.id}
                                label={`Komentar`} />
                        </KorisnikBodyInfoDataWrapperStyled>
                        <Button
                            variant={`contained`}>
                            Sačuvaj
                        </Button>
                </Grid>
            </Grid>
    )
}