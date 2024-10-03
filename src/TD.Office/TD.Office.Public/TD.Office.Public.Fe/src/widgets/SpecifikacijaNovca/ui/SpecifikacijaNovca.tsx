import { useEffect, useState } from 'react'
import { IStoreDto } from '@/dtos/stores/IStoreDto'
import dayjs, { Dayjs } from 'dayjs'
import { useUser } from '@/hooks/useUserHook'
import { handleApiError, officeApi } from '@/apis/officeApi'
import { Grid, LinearProgress } from '@mui/material'
import axios, { AxiosResponse } from 'axios'
import { ISpecificationDto } from '@/dtos/specifications/ISpecificationDto'
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
import { ENDPOINTS } from '@/constants'
import { getUkupnoGotovine } from '@/widgets/SpecifikacijaNovca/helpers/SpecifikacijaHelpers'

export const SpecifikacijaNovca = () => {
    const [selectedStore, setSelectedStore] = useState<IStoreDto | undefined>(
        undefined
    )
    const [date, setDate] = useState<Dayjs>(dayjs(new Date()))
    const [stores, setStores] = useState<IStoreDto[] | undefined>(undefined)
    const [currentSpecification, setCurrentSpecification] = useState<
        ISpecificationDto | undefined
    >(undefined)

    const [putRequest, setPutRequest] = useState<any>({})

    const [isStoreActionSelected, setIsStoreActionSelected] =
        useState<boolean>(false)

    const user = useUser(false)

    const panelsSpacing = 6

    useEffect(() => {
        if (!currentSpecification) setPutRequest({})

        if (currentSpecification)
            setPutRequest({
                specifikacijaNovca: currentSpecification.specifikacijaNovca,
                komentar: currentSpecification.komentar,
            })
    }, [currentSpecification])

    console.log([].reduce)

    useEffect(() => {
        officeApi
            .get(ENDPOINTS.STORES.GET_MULTIPLE)
            .then((response: AxiosResponse) => {
                const storesData = response.data
                setStores(storesData)
                setSelectedStore(
                    storesData.find(
                        (store: IStoreDto) => store.id === user.data?.storeId
                    )
                )
            })
            .catch((err) => handleApiError(err))

        setTimeout(() => {
            setCurrentSpecification({
                id: 524,
                magacinId: 112,
                datumUTC: '2024-07-26T14:55:00.247Z',
                racunar: {
                    gotovinskiRacuni: '45.000,00 RSD',
                    virmanskiRacuni: '24.000,00 RSD',
                    kartice: '17.000,00 RSD',
                    ukupnoRacunar: '86.000,00 RSD',
                    gotovinskePovratnice: '48.000,00 RSD',
                    virmanskePovratnice: '29.000,00 RSD',
                    ostalePovratnice: '54.000,00 RSD',
                },
                poreska: {
                    fiskalizovaniRacuni: '320.00 RSD',
                    fiskalizovanePovratnice: '182.00 RSD',
                },
                specifikacijaNovca: {
                    eur1: {
                        komada: 0,
                        kurs: 117,
                    },
                    eur2: {
                        komada: 0,
                        kurs: 117,
                    },
                    novcanice: [
                        {
                            key: 5000,
                            value: 0,
                        },
                        {
                            key: 2000,
                            value: 0,
                        },
                        {
                            key: 1000,
                            value: 2,
                        },
                        {
                            key: 500,
                            value: 0,
                        },
                        {
                            key: 200,
                            value: 0,
                        },
                        {
                            key: 100,
                            value: 0,
                        },
                        {
                            key: 50,
                            value: 0,
                        },
                        {
                            key: 20,
                            value: 0,
                        },
                        {
                            key: 10,
                            value: 0,
                        },
                        {
                            key: 5,
                            value: 0,
                        },
                        {
                            key: 2,
                            value: 0,
                        },
                        {
                            key: 1,
                            value: 0,
                        },
                    ],
                    ostalo: [
                        {
                            key: 'kartice',
                            vrednost: 0,
                            komentar: 'Kupac platio karticom',
                        },
                        {
                            key: 'cekovi',
                            vrednost: 0,
                            komentar: 'Kupac platio cekovima',
                        },
                        {
                            key: 'papiri',
                            vrednost: 0,
                            komentar: 'Kupac platio papirima',
                        },
                        {
                            key: 'troskovi',
                            vrednost: 0,
                            komentar: 'Kupac ima troskove',
                        },
                        {
                            key: 'vozaci',
                            vrednost: 0,
                            komentar: 'Vozaci duguju puno',
                        },
                        {
                            key: 'sasa',
                            vrednost: 0,
                            komentar: 'Ima para kod Sase',
                        },
                    ],
                },
                komentar: 'Dobra fiskalizacija danas odradjena',
                racunarTrazi: {
                    value: 62000,
                    label: '62.000,00 RSD',
                },
            })
        }, 1000)
    }, [user.data?.storeId])

    const handleSaveSpecificationChanges = () => {
        officeApi
            .put(`/specifications/${currentSpecification?.id}`, putRequest)
            .catch((err) => handleApiError(err))
    }

    const handleSpecifikacijaNovcaGotovinaInputFieldChange = (
        note: number,
        value: number
    ) => {
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

    const handleSpecifikacijaNovcaOstaloDataFieldChange = (
        key: string,
        value: number
    ) => {
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

    const handleKomentarDataFieldChange = (value: string) =>
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
        (currentSpecification?.racunarTrazi.value ?? 0) -
        getUkupnoGotovine(currentSpecification) -
        specifikacijaNovcaOstalo

    return !currentSpecification || !user || !stores ? (
        <Grid
            sx={{
                p: 2,
            }}
        >
            <LinearProgress
                sx={{
                    height: 20,
                    borderRadius: 20,
                }}
            />
        </Grid>
    ) : (
        <Grid
            container
            padding={4}
            spacing={panelsSpacing}
            justifyContent={`center`}
        >
            <SpecifikacijaNovcaTopBarActions
                stores={stores}
                currentStore={selectedStore}
                date={date}
                currentSpecificationNumber={currentSpecification.id}
                onChangeDate={(newDate) => setDate(newDate)}
                onChangeStore={(store) => setSelectedStore(store)}
            />
            <SpecifikacijaNovcaHelperActions
                onStoreButtonClick={() =>
                    setIsStoreActionSelected((prevState) => !prevState)
                }
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
                                    currentSpecification.specifikacijaNovca
                                        .ostalo
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
                racunarTraziLabel={currentSpecification.racunarTrazi.label}
                obracunRazlika={obracunRazlika}
            />
            <SpecifikacijaNovcaSaveButton
                onClick={() => {
                    handleSaveSpecificationChanges()
                }}
            />
        </Grid>
    )
}
