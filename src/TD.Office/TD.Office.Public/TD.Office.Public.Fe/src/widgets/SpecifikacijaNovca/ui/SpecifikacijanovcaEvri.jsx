import { Box, Divider, Grid, Stack, Typography } from '@mui/material'
import { EnchantedTextField } from '../../EnchantedTextField/ui/EnchantedTextField'
import { useState } from 'react'
import { SpecifikacijaNovcaBox } from './SpecifikacijaNovcaBox'
import { formatNumber } from '../../../helpers/numberHelpers'

export const SpecifikacijanovcaEvri = ({
    disabled,
    specifikacijaNovca,
    onChange,
}) => {
    const eur1 = specifikacijaNovca?.eur1
    const eur2 = specifikacijaNovca?.eur2
    const [eur1Komada, setEur1Komada] = useState(eur1?.komada || 0)
    const [eur2Komada, setEur2Komada] = useState(eur2?.komada || 0)
    const [eur1Kurs, setEur1Kurs] = useState(eur1?.kurs || 0)
    const [eur2Kurs, setEur2Kurs] = useState(eur2?.kurs || 0)
    return (
        <SpecifikacijaNovcaBox title={`Specifikacija novca - EUR`}>
            <Stack spacing={2}>
                <EnchantedTextField
                    fullWidth
                    textAlignment="left"
                    label={`EUR 1 - Komada`}
                    value={parseFloat(eur1Komada)}
                    onChange={(e) => {
                        setEur1Komada(parseFloat(e))
                        onChange(0, parseFloat(e), eur1Kurs)
                    }}
                />
                <EnchantedTextField
                    fullWidth
                    textAlignment="left"
                    label={`EUR 1 - Kurs`}
                    value={parseFloat(eur1Kurs)}
                    onChange={(e) => {
                        let val = parseFloat(e)
                        if (isNaN(val)) val = 0
                        setEur1Kurs(val)
                        onChange(0, eur1Komada, val)
                    }}
                />
                <Typography
                    sx={{
                        textAlign: `right`,
                        lineHeight: 0.8,
                    }}
                >
                    = {formatNumber(eur1Komada * eur1Kurs)} EUR
                </Typography>
                <Divider />
                <EnchantedTextField
                    fullWidth
                    textAlignment="left"
                    label={`EUR 2 - Komada`}
                    value={parseFloat(eur2Komada)}
                    onChange={(e) => {
                        setEur2Komada(parseFloat(e))
                        onChange(0, parseFloat(e), eur2Kurs)
                    }}
                />
                <EnchantedTextField
                    fullWidth
                    textAlignment="left"
                    label={`EUR 2 - Kurs`}
                    value={parseFloat(eur2Kurs)}
                    onChange={(e) => {
                        let val = parseFloat(e)
                        if (isNaN(val)) val = 0
                        setEur2Kurs(val)
                        onChange(0, eur2Komada, val)
                    }}
                />
                <Typography
                    sx={{
                        textAlign: `right`,
                        lineHeight: 0.8,
                    }}
                >
                    = {formatNumber(eur2Komada * eur2Kurs)} EUR
                </Typography>
            </Stack>
        </SpecifikacijaNovcaBox>
    )
}
