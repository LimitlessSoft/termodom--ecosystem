import { Chip, Grid, TextField, Typography } from '@mui/material'
import { useEffect, useState } from 'react'
import { mainTheme } from '@/themes'
import { handleApiError, officeApi } from '@/apis/officeApi'

export const AzuriranjeCenaUslovFormiranjaReferentniProizvod = (props) => {
    const [isNew, setIsNew] = useState(false)
    const [currentInput, setCurrentInput] = useState('')
    const [isLoadingSuggestions, setIsLoadingSuggestions] = useState(false)
    const [suggestions, setSuggestions] = useState([])

    const [referentId, setReferentId] = useState(undefined)
    const [referentName, setReferentName] = useState('')

    useEffect(() => {
        if (props.isInitial == null || props.isInitial == false) {
            return
        }
        setReferentId(props.modifikator)
    }, [props.isInitial, props.modifikator])

    useEffect(() => {
        if (referentId != null && referentId != 0) {
            officeApi
                .get(`/web-products?id=${referentId}`)
                .then((response) => {
                    if (response.length == 0) {
                        return
                    }

                    props.onChange(response[0].id)
                    setReferentId(response[0].id)
                    setReferentName(response[0].name)
                })
                .catch((err) => handleApiError(err))
        }
    }, [referentId])

    return (
        <Grid container>
            <Grid item sm={12}>
                <Typography
                    variant={`body2`}
                    sx={{
                        color: mainTheme.palette.success.main,
                        my: 2,
                        fontWeight: 'bold',
                    }}
                >
                    {referentId != null && referentId != 0
                        ? `${isNew ? 'Budući ' : 'Trenutni'} referentni proizvod je: ` +
                          referentName +
                          ` (${referentId})`
                        : `Referentni proizvod još uvek nije postavljen`}
                </Typography>
                <TextField
                    type={`text`}
                    label={`Otkucajte naziv / kataloški broj proizvoda i pritisnite enter`}
                    helperText={`Otkucajte naziv / kataloški broj proizvoda i pritisnite enter`}
                    disabled={isLoadingSuggestions}
                    onChange={(e) => {
                        setCurrentInput(e.target.value)
                    }}
                    onKeyUp={(e) => {
                        if (e.key == 'Enter' || e.key == 'Return') {
                            if (
                                currentInput != null &&
                                currentInput.length > 0
                            ) {
                                setIsLoadingSuggestions(true)
                                officeApi
                                    .get(
                                        `/web-azuriraj-cene-uslov-formiranja-min-web-osnova-product-suggestion?SearchText=${currentInput}`
                                    )
                                    .then((response) => {
                                        setSuggestions(response.data)
                                        setIsLoadingSuggestions(false)
                                    })
                                    .catch((err) => handleApiError(err))
                            }
                        }
                    }}
                    placeholder={`Otkucajte naziv / kataloški broj proizvoda i pritisnite enter`}
                ></TextField>
            </Grid>
            <Grid
                container
                spacing={1}
                sx={{
                    my: 2,
                }}
            >
                {suggestions.map((suggestion) => {
                    return (
                        <Grid item key={suggestion.key}>
                            <Chip
                                label={suggestion.value}
                                variant="outlined"
                                onClick={() => {
                                    setIsNew(true)
                                    setReferentId(suggestion.key)
                                }}
                            />
                        </Grid>
                    )
                })}
            </Grid>
        </Grid>
    )
}
