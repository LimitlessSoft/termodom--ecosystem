import { useEffect, useState } from 'react'
import { useRouter } from 'next/router'
import { handleApiError, officeApi } from '../../../apis/officeApi'
import { DATE_FORMAT, ENDPOINTS_CONSTANTS } from '../../../constants'
import { CircularProgress, Grid, Stack } from '@mui/material'
import moment from 'moment'
import { SpecifikacijaNovcaRacunar } from '../../../widgets/SpecifikacijaNovca/ui/SpecifikacijaNovcaRacunar'
import { SpecifikacijaNovcaPoreska } from '../../../widgets/SpecifikacijaNovca/ui/SpecifikacijaNovcaPoreska'
import { SpecifikacijaNovcaKomentar } from '../../../widgets/SpecifikacijaNovca/ui/SpecifikacijaNovcaKomentar'
import { SpecifikacijaNovcaGotovina } from '../../../widgets/SpecifikacijaNovca/ui/SpecifikacijaNovcaGotovina'
import { SpecifikacijanovcaEvri } from '../../../widgets/SpecifikacijaNovca/ui/SpecifikacijanovcaEvri'
import { SpecifikacijaNovcaOstalo } from '../../../widgets/SpecifikacijaNovca/ui/SpecifikacijaNovcaOstalo'
import { SpecifikacijaNovcaObracun } from '../../../widgets/SpecifikacijaNovca/ui/SpecifikacijaNovcaObracun'
import { EnchantedTextField } from '../../../widgets'
import {
    decryptSpecifikacijaNovcaId,
    getUkupnoGotovine,
} from '../../../widgets/SpecifikacijaNovca/helpers/SpecifikacijaHelpers'

const SpecifikacijaNovcaPrintPage = () => {
    const router = useRouter()
    const { encryptedId } = router.query
    const panelsSpacing = 2

    const [specifikacijaNovcaOstalo, setSpecifikacijaNovcaOstalo] = useState()
    const [obracunRazlika, setObracunRazlika] = useState(0)
    const [currentSpecification, setCurrentSpecification] = useState(null)

    useEffect(() => {
        if (!currentSpecification) return

        setSpecifikacijaNovcaOstalo(
            currentSpecification?.specifikacijaNovca.ostalo.reduce(
                (prevValue, currentValue) => prevValue + currentValue.vrednost,
                0
            ) ?? 0
        )

        setObracunRazlika(
            getUkupnoGotovine(currentSpecification) +
                currentSpecification?.specifikacijaNovca.eur1?.komada *
                    currentSpecification?.specifikacijaNovca.eur1?.kurs +
                currentSpecification?.specifikacijaNovca.eur2?.komada *
                    currentSpecification?.specifikacijaNovca.eur2?.kurs +
                specifikacijaNovcaOstalo -
                (currentSpecification?.racunar.racunarTraziValue ?? 0)
        )
        setTimeout(() => {
            const css = '@page { size: A4; margin: 0; } html { margin: 0; }',
                head =
                    document.head || document.getElementsByTagName('head')[0],
                style = document.createElement('style')

            style.type = 'text/css'
            style.media = 'print'

            if (style.styleSheet) {
                style.styleSheet.cssText = css
            } else {
                style.appendChild(document.createTextNode(css))
            }

            head.appendChild(style)
            // window.print()
        }, 500)
    }, [currentSpecification, specifikacijaNovcaOstalo])

    useEffect(() => {
        if (!encryptedId) return

        const id = decryptSpecifikacijaNovcaId(encryptedId)
        console.log(id)
        officeApi
            .get(ENDPOINTS_CONSTANTS.SPECIFIKACIJA_NOVCA.GET(id))
            .then((response) => {
                setCurrentSpecification(response.data)
            })
            .catch(handleApiError)
    }, [encryptedId])

    if (!currentSpecification) return <CircularProgress />
    return (
        <Grid
            container
            width={`100%`}
            spacing={panelsSpacing}
            style={{
                transform: `translateX(7px)`,
            }}
        >
            <style>{`button { display: none !important; } input { height: 1px !important; } .css-1knfrdf-MuiFormLabel-root-MuiInputLabel-root { top: -10px !important; }`}</style>

            <Grid item xs={12}>
                <Stack gap={2} direction={`row`}>
                    <EnchantedTextField
                        label={`Broj specifikacije`}
                        readOnly
                        value={currentSpecification.id}
                    />
                    <EnchantedTextField
                        label={`Magacin specifikacije`}
                        readOnly
                        value={currentSpecification.magacinId}
                    />
                    <EnchantedTextField
                        label={`Datum specifikacije`}
                        readOnly
                        value={moment(currentSpecification.datumUTC).format(
                            DATE_FORMAT
                        )}
                    />
                </Stack>
            </Grid>
            <Grid item xs={4}>
                <Grid container direction={`column`} spacing={panelsSpacing}>
                    <SpecifikacijaNovcaRacunar
                        racunar={currentSpecification.racunar}
                    />
                    <SpecifikacijaNovcaPoreska
                        poreska={currentSpecification.poreska}
                    />
                    <SpecifikacijaNovcaKomentar
                        disabled={true}
                        komentar={currentSpecification.komentar}
                    />
                </Grid>
            </Grid>
            <Grid item xs={4}>
                <SpecifikacijaNovcaGotovina
                    disableItemSum={true}
                    disabled={true}
                    specifikacija={currentSpecification}
                />
            </Grid>
            <Grid item xs={4}>
                <Grid container gap={panelsSpacing} direction={`column`}>
                    <SpecifikacijanovcaEvri
                        disabled={true}
                        specifikacijaNovca={
                            currentSpecification.specifikacijaNovca
                        }
                    />
                    <SpecifikacijaNovcaOstalo
                        disabled={true}
                        ostalo={currentSpecification.specifikacijaNovca.ostalo}
                    />
                </Grid>
            </Grid>
            <SpecifikacijaNovcaObracun
                racunarTraziLabel={currentSpecification.racunar.racunarTrazi}
                obracunRazlika={obracunRazlika}
                imaNefiskalizovanih={
                    currentSpecification.racunar.imaNefiskalizovanih
                }
            />
        </Grid>
    )
}

export default SpecifikacijaNovcaPrintPage
