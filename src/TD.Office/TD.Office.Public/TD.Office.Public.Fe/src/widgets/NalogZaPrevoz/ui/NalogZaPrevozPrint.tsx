import { CircularProgress, Grid, Typography } from "@mui/material"
import {formatNumber} from "@/helpers/numberHelpers"
import {useUser} from "@/hooks/useUserHook"
import { useEffect } from "react"

export const NalogZaPrevozPrint = (props: any): JSX.Element => {

    const user = useUser(true, true)
    const data = props.data

    useEffect(() => {
        if(data === undefined) return

        setTimeout(() => {
            var css = '@page { size: A4; margin: 0; } html { margin: 0; }',
            head = document.head || document.getElementsByTagName('head')[0],
            style: any = document.createElement('style');
    
            style.type = 'text/css';
            style.media = 'print';
    
            if (style.styleSheet){
                style.styleSheet.cssText = css;
            } else {
                style.appendChild(document.createTextNode(css));
            }
    
            head.appendChild(style);
            window.print()
        }, 500);
    }, [data])

    return (
        <Grid>
            { data === undefined && <CircularProgress /> }
            { data !== undefined &&
                <Grid m={1} container>
                    <Grid item xs={12} px={13} py={4}>
                        <Typography my={1} variant={`h5`} fontWeight={`bolder`}>Nalog za prevoz: {data.id}</Typography>
                        <Typography variant={`body1`}>Adresa: {data.address}</Typography>
                        <Typography variant={`body1`}>Kontakt: {data.mobilni}</Typography>
                        <Typography variant={`body1`}>Prevoznik: {data.prevoznik}</Typography>
                        <Typography variant={`body1`}>Ukupna cena prevoza (bez PDV): {formatNumber(data.cenaPrevozaBezPdv)}</Typography>
                        <Typography variant={`body1`}>Od toga kupcu naplaceno (bez PDV): {formatNumber(data.miNaplatiliKupcuBezPdv)} {data.placenVirmanom && '(virmanom)'}</Typography>
                        <Typography variant={`body1`}>Napomena: {data.note}</Typography>
                        <Typography variant={`body1`} fontWeight={`bolder`}>Veza dokument: {data.vrDok} - {data.brDok}</Typography>
                    </Grid>
                    <Grid item xs={12} my={10}>
                        <Grid container justifyContent={`center`} spacing={4}>
                            {
                                [1, 2, 3, 4].map((item: number) => {
                                    return (
                                        <Grid item key={item} xs={6} textAlign={`center`}>
                                            <Typography variant={`h6`}>_____________________</Typography>
                                            { item === 1 && <Typography>Nalog sastavio</Typography> }
                                            { item === 2 && <Typography>Robu izdao</Typography> }
                                            { item === 3 && <Typography>Robu prevezao</Typography> }
                                            { item === 4 && <Typography>Robu preuzeo</Typography> }
                                        </Grid>
                                    )
                                })
                            }
                        </Grid>
                    </Grid>
                </Grid>
            }
        </Grid>
    )
}