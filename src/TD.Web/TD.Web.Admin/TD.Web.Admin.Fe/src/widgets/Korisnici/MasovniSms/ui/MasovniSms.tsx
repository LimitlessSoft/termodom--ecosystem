import { ApiBase, ContentType, fetchApi } from "@/app/api"
import { Button, CircularProgress, Dialog, DialogActions, DialogContent, DialogTitle, Grid, LinearProgress, TextField, Typography } from "@mui/material"
import { useEffect, useState } from "react"
import { toast } from "react-toastify"

export const MasovniSms = (props: any): JSX.Element => {

    const [isOpen, setIsOpen] = useState<boolean>(false)
    const [text, setText] = useState<string>(``)
    const [pendingConfirm, setPendingConfirm] = useState<boolean>(false)
    const [sending, setSending] = useState<boolean>(false)

    return (
        <Grid item lg={12} my={2}>
            <Dialog open={isOpen} onClose={() => {
                if(sending == true)
                    return

                setIsOpen(false)
            }}>
                <DialogTitle>
                    Pošalji masovni SMS
                </DialogTitle>
                <DialogContent>
                    <TextField
                        disabled={sending}
                        fullWidth
                        label={`Tekst poruke`}
                        multiline
                        rows={4}
                        variant={`outlined`}
                        onChange={(e) => {
                            setText(e.target.value)
                            setPendingConfirm(false)
                        }} />
                    <Typography>
                        Karaktera: {text.length} / 120 (testirati max karaktera)
                    </Typography>
                </DialogContent>
                <DialogActions>
                    <Button disabled={sending} onClick={() => {
                        setIsOpen(false)
                        setPendingConfirm(false)
                    }}>Otkaži</Button>
                    {
                        pendingConfirm == false &&
                            <Button variant={`outlined`} onClick={() => {
                                setPendingConfirm(true)
                            }}>Pošalji</Button>
                    }
                    {
                        pendingConfirm && 
                            <Button disabled={sending} variant={`contained`} onClick={() => {
                                setSending(true)
                                fetchApi(ApiBase.Main, `/users-send-sms`, {
                                    body: {
                                        text: text,
                                        favoriteStoreId: props.currentFilter.filteredStore == -1 ? null : props.currentFilter.filteredStore,
                                        cityId: props.currentFilter.filteredCity == -1 ? null : props.currentFilter.filteredCity,
                                        professionId: props.currentFilter.filteredProfession == -1 ? null : props.currentFilter.filteredProfession,
                                        userType: props.currentFilter.filteredType == -1 ? null : props.currentFilter.filteredType,
                                        isActive: props.currentFilter.filteredStatus == 0 ? null : props.currentFilter.filteredStatus == 1
                                    },
                                    method: `POST`,
                                    contentType: ContentType.ApplicationJson
                                }).then(() => {
                                    toast.success(`SMS poruke uspešno poslate!`)
                                }).finally(() => {
                                    setIsOpen(false)
                                    setPendingConfirm(false)
                                    setSending(false)
                                })
                            }}>
                                {
                                    sending == false ?
                                        `Da, potvrđujem, pošalji!` :
                                        `Slanje u toku...`
                                }
                                {
                                    sending && <CircularProgress size={`1em`} sx={{ marginLeft: 2 }} />
                                }
                            </Button>
                    }

                </DialogActions>
            </Dialog>

            <Button variant={`contained`} onClick={() => {
                setIsOpen(true)
            }}>Pošalji masovni SMS izlistanim korisnicima</Button>
        </Grid>
    )
}