import { useState } from 'react'
import dayjs from 'dayjs'
import { handleApiError, officeApi } from '@/apis/officeApi'
import { Grid } from '@mui/material'
import { toast } from 'react-toastify'
import { SpecifikacijaNovcaRacunar } from './SpecifikacijaNovcaRacunar'
import { SpecifikacijaNovcaPoreska } from './SpecifikacijaNovcaPoreska'
import { SpecifikacijaNovcaOstalo } from './SpecifikacijaNovcaOstalo'
import { SpecifikacijaNovcaGotovina } from './SpecifikacijaNovcaGotovina'
import { SpecifikacijaNovcaHelperActions } from './SpecifkacijaNovcaHelperActions'
import { SpecifikacijaNovcaObracun } from './SpecifikacijaNovcaObracun'
import { SpecifikacijaNovcaTopBarActions } from './SpecifikacijaNovcaTopBarActions'
import { SpecifikacijaNovcaKomentar } from './SpecifikacijaNovcaKomentar'
import { SpecifikacijaNovcaSaveButton } from './SpecikacijaNovcaSaveButton'
import { PERMISSIONS_CONSTANTS } from '@/constants'
import { getUkupnoGotovine } from '@/widgets/SpecifikacijaNovca/helpers/SpecifikacijaHelpers'
import { usePermissions } from '@/hooks/usePermissionsHook'
import { ENDPOINTS_CONSTANTS } from '../../../constants'

export const SpecifikacijaNovca = () => {
    const [currentSpecification, setCurrentSpecification] = useState()

    const [putRequest, setPutRequest] = useState({})

    const [isStoreActionSelected, setIsStoreActionSelected] = useState(false)

    const permissions = usePermissions(
        PERMISSIONS_CONSTANTS.PERMISSIONS_GROUPS.SPECIFIKACIJA_NOVCA
    )

    const panelsSpacing = 6

    // useEffect(() => {
    //     if (!currentSpecification) setPutRequest({})

    //     if (currentSpecification)
    //         setPutRequest({
    //             specifikacijaNovca: currentSpecification.specifikacijaNovca,
    //             komentar: currentSpecification.komentar,
    //         })
    // }, [currentSpecification])

    const handleSaveSpecificationChanges = async () => {
        console.log(currentSpecification)
        await officeApi
            .put(
                ENDPOINTS_CONSTANTS.SPECIFIKACIJA_NOVCA.SAVE(
                    currentSpecification.id
                ),
                currentSpecification
            )
            .catch(handleApiError)
    }

    const handleSpecifikacijaNovcaGotovinaInputFieldChange = (note, value) => {
        setCurrentSpecification((prevState) => {
            if (!prevState) {
                toast.error(
                    'Greska prilikom izmene specifikacije novca. Osvezite stranicu.'
                )
                return prevState
            }

            return {
                ...prevState,
                specifikacijaNovca: {
                    ...prevState?.specifikacijaNovca,
                    novcanice: prevState.specifikacijaNovca.novcanice.map(
                        (novcanica) =>
                            novcanica.key === note
                                ? { ...novcanica, value }
                                : novcanica
                    ),
                },
            }
        })
    }

    const handleSpecifikacijaNovcaOstaloDataFieldChange = (key, value) => {
        setCurrentSpecification((prevState) => {
            if (!prevState) return prevState

            return {
                ...prevState,
                specifikacijaNovca: {
                    ...prevState.specifikacijaNovca,
                    ostalo: prevState.specifikacijaNovca.ostalo.map((field) =>
                        field.key === key
                            ? { ...field, vrednost: value }
                            : field
                    ),
                },
            }
        })
    }

    const handleKomentarDataFieldChange = (value) =>
        setCurrentSpecification(
            (prevState) =>
                prevState && {
                    ...prevState,
                    komentar: value,
                }
        )

    const specifikacijaNovcaOstalo =
        currentSpecification?.specifikacijaNovca.ostalo.reduce(
            (prevValue, currentValue) => prevValue + currentValue.vrednost,
            0
        ) ?? 0

    const obracunRazlika =
        (currentSpecification?.racunar.racunarTrazi ?? 0) -
        getUkupnoGotovine(currentSpecification) -
        specifikacijaNovcaOstalo

    return (
        <Grid
            container
            padding={4}
            spacing={panelsSpacing}
            justifyContent={`center`}
        >
            <SpecifikacijaNovcaTopBarActions
                permissions={permissions}
                onDataChange={(data) => {
                    setCurrentSpecification(data)
                }}
            />
            {currentSpecification && (
                <>
                    <SpecifikacijaNovcaHelperActions
                        permissions={permissions}
                        onStoreButtonClick={() =>
                            setIsStoreActionSelected((prevState) => !prevState)
                        }
                        date={dayjs(currentSpecification.datum)}
                        isStoreButtonSelected={isStoreActionSelected}
                    />
                    <Grid item>
                        <Grid container direction={`column`} spacing={2}>
                            <SpecifikacijaNovcaRacunar
                                racunar={currentSpecification.racunar}
                            />
                            <SpecifikacijaNovcaPoreska
                                poreska={currentSpecification.poreska}
                            />
                            <SpecifikacijaNovcaKomentar
                                komentar={currentSpecification.komentar}
                                onChange={handleKomentarDataFieldChange}
                            />
                        </Grid>
                    </Grid>
                    <Grid item>
                        <Grid container spacing={panelsSpacing}>
                            <Grid item xs={12}>
                                <Grid container spacing={panelsSpacing}>
                                    <SpecifikacijaNovcaGotovina
                                        specifikacija={currentSpecification}
                                        onChange={
                                            handleSpecifikacijaNovcaGotovinaInputFieldChange
                                        }
                                    />
                                    <SpecifikacijaNovcaOstalo
                                        ostalo={
                                            currentSpecification
                                                .specifikacijaNovca.ostalo
                                        }
                                        onChange={
                                            handleSpecifikacijaNovcaOstaloDataFieldChange
                                        }
                                    />
                                </Grid>
                            </Grid>
                        </Grid>
                    </Grid>
                    <SpecifikacijaNovcaObracun
                        racunarTraziLabel={
                            currentSpecification.racunar.racunarTrazi
                        }
                        obracunRazlika={obracunRazlika}
                    />
                    <SpecifikacijaNovcaSaveButton
                        permissions={permissions}
                        onClick={() => {
                            handleSaveSpecificationChanges()
                        }}
                    />
                </>
            )}
        </Grid>
    )
}
