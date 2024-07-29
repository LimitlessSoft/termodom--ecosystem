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
import { EnchantedTextField } from '@/widgets'

export const SpecifikacijaNovca = () => {
    
    const [selectedStore, setSelectedStore] = useState<IStoreDto | undefined>(undefined)
    const [date, setDate] = useState<Dayjs>(dayjs(new Date()))
    
    const [stores, setStores] = useState<IStoreDto[] | undefined>(undefined)
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
                            value: 0,
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
                    value: 58000,
                    label: '58.000,00 RSD',
                },
            })
        }, 1000)
    }, [])

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

    const hanldeSpecifikacijaNovcaOstaloDataFieldChange = (
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

    const ukupnoGotovine =
        currentSpecification?.specifikacijaNovca.novcanice.reduce(
            (prevNovcanica, currentNovcanica) =>
                prevNovcanica +
                currentNovcanica.value * // TODO: check if can without regex
                currentNovcanica.key,
            0
        ) ?? 0

    const specifikacijaNovcaOstalo =
        currentSpecification?.specifikacijaNovca.ostalo.reduce(
            (prevValue, currentValue) =>
                prevValue +
                currentValue.vrednost, // TODO: Check if we can without regex
            0
        ) ?? 0

    const obracunRazlika =
        (currentSpecification?.racunarTrazi.value ?? 0) - ukupnoGotovine - specifikacijaNovcaOstalo

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
            <Grid item xs={12}>
                <Grid container spacing={2} alignItems={`center`}>
                    <Grid item xs={4}>
                        {stores && stores.length > 0 && (
                            <Autocomplete
                                value={stores.find(
                                    (store) => store.id === user.data!.storeId
                                )}
                                options={stores}
                                onChange={(event, store) =>
                                    setSelectedStore(store ?? undefined)
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
                        <EnchantedTextField
                            label={`Pretraga po broju specifikacije`}
                            defaultValue={0}
                            inputType={`number`}
                        />
                    </Grid>
                    <Grid item>
                        <EnchantedTextField
                            label={`Broj specifikacije`}
                            readOnly
                            defaultValue={0}
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
                                <EnchantedTextField
                                    readOnly
                                    label={`1) Gotovinski racuni:`}
                                    defaultValue={
                                        currentSpecification.racunar
                                            .gotovinskiRacuni
                                    }
                                />
                                <EnchantedTextField
                                    readOnly
                                    label={`2) Virmanski racuni:`}
                                    defaultValue={
                                        currentSpecification.racunar
                                            .virmanskiRacuni
                                    }
                                />
                                <EnchantedTextField
                                    readOnly
                                    label={`3) Kartice:`}
                                    defaultValue={currentSpecification.racunar.kartice}
                                />
                                <EnchantedTextField
                                    readOnly
                                    label={`Ukupno racunar (1+2+3):`}
                                    defaultValue={
                                        currentSpecification.racunar
                                            .ukupnoRacunar
                                    }
                                />
                                <EnchantedTextField
                                    readOnly
                                    label={`Gotovinske povratnice:`}
                                    defaultValue={
                                        currentSpecification.racunar
                                            .gotovinskePovratnice
                                    }
                                />
                                <EnchantedTextField
                                    readOnly
                                    label={`Virmanske povratnice:`}
                                    defaultValue={
                                        currentSpecification.racunar
                                            .virmanskePovratnice
                                    }
                                />
                                <EnchantedTextField
                                    readOnly
                                    label={`Ostale povratnice:`}
                                    defaultValue={
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
                                    <EnchantedTextField
                                        readOnly
                                        label={`Fiskalizovani racuni:`}
                                        defaultValue={
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
                                    <EnchantedTextField
                                        readOnly
                                        label={`Fiskalizovane povratnice:`}
                                        defaultValue={
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
                                        {currentSpecification.specifikacijaNovca
                                            .novcanice &&
                                            currentSpecification.specifikacijaNovca.novcanice.map(
                                                (novcanica, i) => {
                                                    return (
                                                        <SpecifikacijaNovcaGotovinaInputField
                                                            key={i}
                                                            note={novcanica.key}
                                                            gotovinaReference={(
                                                                novcanica.key *
                                                                    novcanica.value
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
                                                                    parseFloat(value)
                                                                )
                                                            }}
                                                        />
                                                    )
                                                }
                                            )}
                                        <EnchantedTextField
                                            label={`Ukupno gotovine:`}
                                            value={ukupnoGotovine}
                                            readOnly
                                            formatValue
                                            inputType={`number`}
                                            formatValueSuffix={` RSD`}
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
                                                        <EnchantedTextField
                                                            textAlignment={`left`}
                                                            inputType={`number`}
                                                            allowDecimal
                                                            label={`${
                                                                field.key
                                                                    .charAt(0)
                                                                    .toUpperCase() +
                                                                field.key.slice(
                                                                    1
                                                                )
                                                            }:`}
                                                            defaultValue={
                                                                field.vrednost
                                                            }
                                                            onChange={(e: string) =>
                                                                hanldeSpecifikacijaNovcaOstaloDataFieldChange(
                                                                    field.key,
                                                                    parseFloat(e)
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
                            <EnchantedTextField
                                readOnly
                                label={`Racunar trazi:`}
                                value={currentSpecification.racunarTrazi.label}
                            />
                        </Grid>
                        <Grid item>
                            <EnchantedTextField
                                readOnly
                                label={`Razlika:`}
                                value={obracunRazlika}
                                formatValue
                                formatValueSuffix={` RSD`}
                                inputType={`number`}
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