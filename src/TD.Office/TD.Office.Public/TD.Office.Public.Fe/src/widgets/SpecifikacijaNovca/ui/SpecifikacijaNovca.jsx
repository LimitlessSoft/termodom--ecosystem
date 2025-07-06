import { useEffect, useState } from 'react'
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
    const [saving, setSaving] = useState(false)
    const [currentSpecification, setCurrentSpecification] = useState()
    const [pendingChanges, setPendingChanges] = useState(false)
    const [isStoreActionSelected, setIsStoreActionSelected] = useState(false)
    const permissions = usePermissions(
        PERMISSIONS_CONSTANTS.PERMISSIONS_GROUPS.SPECIFIKACIJA_NOVCA
    )
    const [specifikacijaNovcaOstalo, setSpecifikacijaNovcaOstalo] = useState()
    const [obracunRazlika, setObracunRazlika] = useState(0)
    const panelsSpacing = 4

    const handleSaveSpecificationChanges = async () => {
        setSaving(true)
        await officeApi
            .put(
                ENDPOINTS_CONSTANTS.SPECIFIKACIJA_NOVCA.SAVE(
                    currentSpecification.id
                ),
                currentSpecification
            )
            .catch(handleApiError)
            .then(() => {
                toast.success(`Specifikacija novca sacuvana.`)
            })
            .finally(() => {
                setSaving(false)
            })
        setPendingChanges(false)
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
        setPendingChanges(true)
    }
    const handleSpecifikacijaNovcaOstaloKomentarChange = (key, komentar) => {
        setCurrentSpecification((prevState) => {
            if (!prevState) return prevState

            return {
                ...prevState,
                specifikacijaNovca: {
                    ...prevState.specifikacijaNovca,
                    ostalo: prevState.specifikacijaNovca.ostalo.map((field) =>
                        field.key === key ? { ...field, komentar } : field
                    ),
                },
            }
        })
        setPendingChanges(true)
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
        setPendingChanges(true)
    }
    const handleKomentarDataFieldChange = (value) => {
        setCurrentSpecification(
            (prevState) =>
                prevState && {
                    ...prevState,
                    komentar: value,
                }
        )
        setPendingChanges(true)
    }

    useEffect(() => {
        setSpecifikacijaNovcaOstalo(
            currentSpecification?.specifikacijaNovca.ostalo.reduce(
                (prevValue, currentValue) => prevValue + currentValue.vrednost,
                0
            ) ?? 0
        )

        setObracunRazlika(
            (currentSpecification?.racunar.racunarTraziValue ?? 0) -
                getUkupnoGotovine(currentSpecification) -
                specifikacijaNovcaOstalo
        )
    }, [currentSpecification, specifikacijaNovcaOstalo])

    return (
        <Grid
            container
            padding={4}
            spacing={panelsSpacing}
            justifyContent={`center`}
        >
            <SpecifikacijaNovcaTopBarActions
                disabled={saving}
                permissions={permissions}
                onDataChange={(data) => {
                    setCurrentSpecification(data)
                }}
            />
            {currentSpecification && (
                <>
                    <SpecifikacijaNovcaHelperActions
                        disabled={saving}
                        permissions={permissions}
                        onStoreButtonClick={() =>
                            setIsStoreActionSelected((prevState) => !prevState)
                        }
                        date={dayjs(currentSpecification.datumUTC)}
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
                                disabled={saving}
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
                                        disabled={saving}
                                        specifikacija={currentSpecification}
                                        onChange={
                                            handleSpecifikacijaNovcaGotovinaInputFieldChange
                                        }
                                    />
                                    <SpecifikacijaNovcaOstalo
                                        disabled={saving}
                                        ostalo={
                                            currentSpecification
                                                .specifikacijaNovca.ostalo
                                        }
                                        onChange={
                                            handleSpecifikacijaNovcaOstaloDataFieldChange
                                        }
                                        onKomentarChange={
                                            handleSpecifikacijaNovcaOstaloKomentarChange
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
                        imaNefiskalizovanih={
                            currentSpecification.racunar.imaNefiskalizovanih
                        }
                    />
                    <SpecifikacijaNovcaSaveButton
                        disabled={!pendingChanges || saving}
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
