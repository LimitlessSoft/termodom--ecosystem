import { useEffect, useState } from 'react'
import { IStoreDto } from '@/dtos/stores/IStoreDto'
import dayjs, { Dayjs } from 'dayjs'
import { useUser } from '@/hooks/useUserHook'
import { officeApi } from '@/apis/officeApi'
import {
    Autocomplete,
    Button,
    CircularProgress,
    Grid,
    LinearProgress,
    Stack,
    TextField,
    Typography,
} from '@mui/material'
import { DatePicker } from '@mui/x-date-pickers'
import {
    ArrowBackIos,
    ArrowForwardIos,
    Bolt,
    Comment,
    Help,
    Print,
} from '@mui/icons-material'
import { AxiosResponse } from 'axios'
import { SpecifikacijaNovcaTopBarButton } from '@/widgets/SpecifikacijaNovca/ui/SpecifikacijaNovcaTopBarButton'
import { SpecifikacijaNovcaDataField } from '@/widgets/SpecifikacijaNovca/ui/SpecifikacijaNovcaDataField'
import { SpecifikacijaNovcaBox } from '@/widgets/SpecifikacijaNovca/ui/SpecifikacijaNovcaBox'
import { SpecifikacijaNovcaGotovinaInputField } from '@/widgets/SpecifikacijaNovca/ui/SpecifikacijaNovcaGotovinaInputField'
import { mainTheme } from '@/themes'
import { ISpecificationDto } from '@/dtos/specifications/ISpecificationDto'
import { toast } from 'react-toastify'

export const SpecifikacijaNovca = () => {
    const [stores, setStores] = useState<IStoreDto[] | undefined>(undefined)
    const [selectedStore, setSelectedStore] = useState<IStoreDto | null>(null)
    const [date, setDate] = useState<Dayjs>(dayjs(new Date()))
    const [currentSpecification, setCurrentSpecification] = useState<
        ISpecificationDto | undefined
    >(undefined)

    const user = useUser(false)

    const panelsSpacing = 6

    useEffect(() => {
        officeApi
            .get('/stores')
            .then((response: AxiosResponse) => setStores(response.data))

        setTimeout(() => {
            setCurrentSpecification({
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
                        komada: 200,
                        kurs: 117,
                    },
                    eur2: {
                        komada: 500,
                        kurs: 117,
                    },
                    novcanice: [
                        {
                            key: 5000,
                            value: '20',
                        },
                        {
                            key: 2000,
                            value: '14',
                        },
                        {
                            key: 1000,
                            value: '7',
                        },
                        {
                            key: 500,
                            value: '25',
                        },
                        {
                            key: 200,
                            value: '12',
                        },
                        {
                            key: 100,
                            value: '8',
                        },
                        {
                            key: 50,
                            value: '4',
                        },
                        {
                            key: 20,
                            value: '9',
                        },
                        {
                            key: 10,
                            value: '3',
                        },
                        {
                            key: 5,
                            value: '16',
                        },
                        {
                            key: 2,
                            value: '5',
                        },
                        {
                            key: 1,
                            value: '5',
                        },
                    ],
                    ostalo: [
                        {
                            key: 'kartice',
                            vrednost: '20000',
                            komentar: 'Kupac platio karticom',
                        },
                        {
                            key: 'cekovi',
                            vrednost: '24000',
                            komentar: 'Kupac platio cekovima',
                        },
                        {
                            key: 'papiri',
                            vrednost: '15000',
                            komentar: 'Kupac platio papirima',
                        },
                        {
                            key: 'troskovi',
                            vrednost: '12000',
                            komentar: 'Kupac ima troskove',
                        },
                        {
                            key: 'vozaci',
                            vrednost: '27000',
                            komentar: 'Vozaci duguju puno',
                        },
                        {
                            key: 'sasa',
                            vrednost: '13000',
                            komentar: 'Ima para kod Sase',
                        },
                    ],
                },
                komentar: 'Dobra fiskalizacija danas odradjena',
                racunarTrazi: {
                    value: 58000,
                    label: '58.000,00 RSD'
                },
            })
        }, 1000)
    }, [])

    const handleSpecifikacijaNovcaGotovinaInputFieldChange = (
        note: number,
        value: string
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

    const hanldeSpecifikacijaNovcaOstaloDataFieldChange = (
        key: string,
        value: string
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

    const ukupnoGotovine =
        currentSpecification?.specifikacijaNovca.novcanice.reduce(
            (prevNovcanica, currentNovcanica) =>
                prevNovcanica +
                parseFloat(currentNovcanica.value.replace(/,/g, '.')) * // TODO: check if can without regex
                    currentNovcanica.key,
            0
        ) ?? 0

    const specifikacijaNovcaOstalo =
        currentSpecification?.specifikacijaNovca.ostalo.reduce(
            (prevValue, currentValue) =>
                prevValue +
                parseFloat(currentValue.vrednost.replace(/,/g, '.')), // TODO: Check if we can without regex
            0
        ) ?? 0

    const obracunRazlika =
        currentSpecification?.racunarTrazi.value ??
        0 - ukupnoGotovine + specifikacijaNovcaOstalo

    return currentSpecification === undefined || user === undefined ? (
        <Grid sx={{
            p: 2
        }}>
            <LinearProgress sx={{
                height: 20,
                borderRadius: 20
            }} />
        </Grid>
    ) : (
        <Grid
            container
            padding={4}
            spacing={panelsSpacing}
            justifyContent={`center`}
        >
            <Grid item xs={12}>
                <Grid container spacing={2} alignItems={`center`}>
                    <Grid item xs={4}>
                        {stores === undefined && <CircularProgress />}
                        {stores && stores.length > 0 && (
                            <Autocomplete
                                value={stores.find(
                                    (store) => store.id === user.data!.storeId
                                )}
                                options={stores}
                                onChange={(event, store) =>
                                    setSelectedStore(store)
                                }
                                getOptionLabel={(option) => {
                                    return `[ ${option.id} ] ${option.name}`
                                }}
                                renderInput={(params) => (
                                    <TextField
                                        variant={`outlined`}
                                        {...params}
                                        label="Magacini"
                                    />
                                )}
                            />
                        )}
                    </Grid>
                    <Grid item>
                        <DatePicker
                            label={`Datum`}
                            onChange={(newDate) => newDate && setDate(newDate)}
                            value={date}
                        />
                    </Grid>
                    <Grid item>
                        <SpecifikacijaNovcaTopBarButton text={`Osvezi`} />
                    </Grid>
                    <Grid item flexGrow={1}></Grid>
                    <Grid item>
                        <SpecifikacijaNovcaDataField
                            label={`Pretraga po broju specifikacije`}
                            value={0}
                        />
                    </Grid>
                    <Grid item>
                        <SpecifikacijaNovcaDataField
                            label={`Broj specifikacije`}
                            readOnly
                            value={0}
                        />
                    </Grid>
                </Grid>
            </Grid>
            <Grid item xs={12}>
                <Grid container justifyContent={`end`} gap={2}>
                    <Grid item>
                        <SpecifikacijaNovcaTopBarButton
                            text={`Help`}
                            startIcon={<Help />}
                        />
                    </Grid>
                    <Grid item>
                        <SpecifikacijaNovcaTopBarButton
                            text={`Stampa`}
                            startIcon={<Print />}
                        />
                    </Grid>
                    <Grid item sm={1}></Grid>
                    <Grid item>
                        <Grid container spacing={1}>
                            <Grid item>
                                <SpecifikacijaNovcaTopBarButton>
                                    <ArrowBackIos
                                        style={{ transform: 'translateX(4px)' }}
                                    />
                                </SpecifikacijaNovcaTopBarButton>
                            </Grid>
                            <Grid item>
                                <SpecifikacijaNovcaTopBarButton
                                    text={`M`}
                                    typographySx={{
                                        fontWeight: `bold`,
                                    }}
                                />
                            </Grid>
                            <Grid item>
                                <SpecifikacijaNovcaTopBarButton>
                                    <ArrowForwardIos />
                                </SpecifikacijaNovcaTopBarButton>
                            </Grid>
                        </Grid>
                    </Grid>
                </Grid>
            </Grid>
            <Grid item>
                <Grid container direction={`column`} spacing={2}>
                    <SpecifikacijaNovcaBox title={`Racunar`}>
                        {currentSpecification.racunar && (
                            <Stack spacing={2}>
                                <SpecifikacijaNovcaDataField
                                    readOnly
                                    label={`1) Gotovinski racuni:`}
                                    value={
                                        currentSpecification.racunar
                                            .gotovinskiRacuni
                                    }
                                />
                                <SpecifikacijaNovcaDataField
                                    readOnly
                                    label={`2) Virmanski racuni:`}
                                    value={
                                        currentSpecification.racunar
                                            .virmanskiRacuni
                                    }
                                />
                                <SpecifikacijaNovcaDataField
                                    readOnly
                                    label={`3) Kartice:`}
                                    value={
                                        currentSpecification.racunar.kartice
                                    }
                                />
                                <SpecifikacijaNovcaDataField
                                    readOnly
                                    label={`Ukupno racunar (1+2+3):`}
                                    value={
                                        currentSpecification.racunar
                                            .ukupnoRacunar
                                    }
                                />
                                <SpecifikacijaNovcaDataField
                                    readOnly
                                    label={`Gotovinske povratnice:`}
                                    value={
                                        currentSpecification.racunar
                                            .gotovinskePovratnice
                                    }
                                />
                                <SpecifikacijaNovcaDataField
                                    readOnly
                                    label={`Virmanske povratnice:`}
                                    value={
                                        currentSpecification.racunar
                                            .virmanskePovratnice
                                    }
                                />
                                <SpecifikacijaNovcaDataField
                                    readOnly
                                    label={`Ostale povratnice:`}
                                    value={
                                        currentSpecification.racunar
                                            .ostalePovratnice
                                    }
                                />
                            </Stack>
                        )}
                    </SpecifikacijaNovcaBox>
                    <SpecifikacijaNovcaBox title={`Poreska`}>
                        {currentSpecification.poreska && (
                            <Stack spacing={2}>
                                <Grid
                                    container
                                    spacing={2}
                                    alignItems={`center`}
                                >
                                    <SpecifikacijaNovcaDataField
                                        readOnly
                                        label={`Fiskalizovani racuni:`}
                                        value={
                                            currentSpecification.poreska
                                                .fiskalizovaniRacuni
                                        }
                                    />
                                    <Grid item>
                                        <Button variant={`contained`}>
                                            <Bolt />
                                        </Button>
                                    </Grid>
                                </Grid>
                                <Grid
                                    container
                                    spacing={2}
                                    alignItems={`center`}
                                >
                                    <SpecifikacijaNovcaDataField
                                        readOnly
                                        label={`Fiskalizovane povratnice:`}
                                        value={
                                            currentSpecification.poreska
                                                .fiskalizovanePovratnice
                                        }
                                    />
                                    <Grid item>
                                        <Button variant={`contained`}>
                                            <Bolt />
                                        </Button>
                                    </Grid>
                                </Grid>
                            </Stack>
                        )}
                    </SpecifikacijaNovcaBox>
                    <SpecifikacijaNovcaBox title={`Komentar`}>
                        {currentSpecification.komentar && (
                            <SpecifikacijaNovcaDataField
                                onChange={handleKomentarDataFieldChange}
                                multiline
                                value={currentSpecification.komentar}
                            />
                        )}
                    </SpecifikacijaNovcaBox>
                </Grid>
            </Grid>
            <Grid item>
                <Grid container spacing={panelsSpacing}>
                    <Grid item xs={12}>
                        <Grid container spacing={panelsSpacing}>
                            <Grid item>
                                <SpecifikacijaNovcaBox
                                    title={`Specifikacija novca - gotovina`}
                                >
                                    <Stack spacing={2}>
                                        {currentSpecification
                                                .specifikacijaNovca.novcanice &&
                                            currentSpecification.specifikacijaNovca.novcanice.map(
                                                (novcanica, i) => {
                                                    return (
                                                        <SpecifikacijaNovcaGotovinaInputField
                                                            key={i}
                                                            note={novcanica.key}
                                                            gotovinaReference={(
                                                                novcanica.key *
                                                                parseFloat(
                                                                    novcanica.value.replace(
                                                                        /,/g,
                                                                        '.'
                                                                    )
                                                                )
                                                            ).toString()}
                                                            value={
                                                                novcanica.value
                                                            }
                                                            onChange={(
                                                                note: number,
                                                                value: string
                                                            ) => {
                                                                handleSpecifikacijaNovcaGotovinaInputFieldChange(
                                                                    note,
                                                                    value
                                                                )
                                                            }}
                                                        />
                                                    )
                                                }
                                            )}
                                        <SpecifikacijaNovcaDataField
                                            label={`Ukupno gotovine:`}
                                            value={ukupnoGotovine}
                                            readOnly
                                        />
                                    </Stack>
                                </SpecifikacijaNovcaBox>
                            </Grid>
                            <Grid item>
                                <SpecifikacijaNovcaBox
                                    title={`Specifikacija Novca - Ostalo`}
                                >
                                    {currentSpecification.specifikacijaNovca
                                        .ostalo && (
                                        <Stack gap={2}>
                                            {currentSpecification.specifikacijaNovca.ostalo.map(
                                                (field, index) => (
                                                    <Grid
                                                        key={index}
                                                        container
                                                        spacing={2}
                                                        alignItems={`center`}
                                                    >
                                                        <SpecifikacijaNovcaDataField
                                                            label={`${
                                                                field.key
                                                                    .charAt(0)
                                                                    .toUpperCase() +
                                                                field.key.slice(
                                                                    1
                                                                )
                                                            }:`}
                                                            value={
                                                                field.vrednost
                                                            }
                                                            onChange={(e) =>
                                                                hanldeSpecifikacijaNovcaOstaloDataFieldChange(
                                                                    field.key,
                                                                    e
                                                                )
                                                            }
                                                        />
                                                        <Grid item>
                                                            <Button
                                                                variant={`contained`}
                                                            >
                                                                <Comment />
                                                            </Button>
                                                        </Grid>
                                                    </Grid>
                                                )
                                            )}
                                        </Stack>
                                    )}
                                </SpecifikacijaNovcaBox>
                            </Grid>
                        </Grid>
                    </Grid>
                </Grid>
            </Grid>
            <Grid item xs={12}>
                <SpecifikacijaNovcaBox title={`Obracun`}>
                    <Typography
                        textAlign={`center`}
                        color={mainTheme.palette.error.main}
                        fontSize={mainTheme.typography.h4.fontSize}
                    >
                        Imate nefiskalizovanih racuna ili povratnica u racunaru
                    </Typography>
                    <Grid container spacing={2} my={3} justifyContent={`end`}>
                        <Grid item>
                            <SpecifikacijaNovcaDataField
                                readOnly
                                label={`Racunar trazi:`}
                                value={currentSpecification.racunarTrazi.label}
                            />
                        </Grid>
                        <Grid item>
                            <SpecifikacijaNovcaDataField
                                readOnly
                                label={`Razlika:`}
                                value={obracunRazlika}
                            />
                        </Grid>
                    </Grid>
                </SpecifikacijaNovcaBox>
            </Grid>
            <Grid item sm={12} textAlign={`right`}>
                <Button
                    variant={`contained`}
                    size={`large`}
                    sx={{
                        fontSize: mainTheme.typography.h5.fontSize,
                    }}
                >
                    Sacuvaj specifikaciju
                </Button>
            </Grid>
        </Grid>
    )
}
