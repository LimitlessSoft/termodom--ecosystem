import { ApiBase, fetchApi } from "@/app/api"
import { Chip, Grid, TextField } from "@mui/material"
import { useState } from "react"

export const AzuriranjeCenaUslovFormiranjaReferentniProizvod = (props: any): JSX.Element => {

    const [isLoadingSuggestions, setIsLoadingSuggestions] = useState(false)
    const [suggestions, setSuggestions] = useState<any[]>([])
    
    return (
        <Grid container>
            <Grid item sm={12}>
                <TextField
                    type={`text`}
                    defaultValue={props.modifikator}
                    label={`Referentni proizvod`}
                    disabled={isLoadingSuggestions}
                    onChange={(e) => {
                        if(e.target.value != null && e.target.value.length >= 6) {
                            setIsLoadingSuggestions(true)
                            fetchApi(ApiBase.Main, `/web-azuriraj-cene-uslov-formiranja-min-web-osnova-product-suggestion?SearchText=${e.target.value}`)
                            .then((response) => {
                                setSuggestions(response)
                                setIsLoadingSuggestions(false)
                                e.target.focus()
                            })
                        }
                        // setRequest({
                        //     ...request,
                        //     modifikator: Number(e.target.value)
                        // })
                    }}
                    placeholder={`Započnite kucanje naziva proizvoda...`}>
                </TextField>
            </Grid>
            <Grid container spacing={1} sx={{
                my: 2
            }}>
                {
                    suggestions.map((suggestion) => {
                        return (
                            <Grid item key={suggestion.key}>
                                <Chip label={suggestion.value} variant="outlined" onClick={() => {
                                    // setRequest({
                                    //     ...request,
                                    //     modifikator: suggestion.key
                                    // })
                                }} />
                            </Grid>
                        )
                    })
                }
            </Grid>
        </Grid>
    )
}