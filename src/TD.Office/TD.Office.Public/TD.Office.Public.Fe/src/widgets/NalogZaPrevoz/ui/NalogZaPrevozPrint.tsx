import { formatNumber } from "@/app/Helpers/numberHelpers"
import { Button, CircularProgress, Grid, Typography } from "@mui/material"
import { useEffect } from "react"

export const NalogZaPrevozPrint = (props: any): JSX.Element => {

    const data = props.data

    useEffect(() => {
        if(data === undefined) return

        setTimeout(() => {
            var css = '@page { size: 210mm 148.5mm; margin: 0; }',
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
                    <Grid item xs={12}>
                        <Typography my={1} variant={`h5`} fontWeight={`bolder`}>Nalog za prevoz: {data.id}</Typography>
                        <Typography variant={`h6`}>Adresa: {data.address}</Typography>
                        <Typography variant={`h6`}>Kontakt: {data.mobilni}</Typography>
                        <Typography variant={`h6`}>Kupcu naplacen prevoz: {formatNumber(data.miNaplatiliKupcuBezPdv)}</Typography>
                        <Typography variant={`h6`}>Napomena: {data.note}</Typography>
                        <Typography variant={`h6`} fontWeight={`bolder`}>Veza dokument: {data.vrDok} - {data.brDok}</Typography>
                    </Grid>
                    <Grid item xs={12} my={10}>
                        <Grid container justifyContent={`center`} spacing={5}>
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