import { useEffect, useState } from 'react'
import { IStoreDto } from '@/dtos/stores/IStoreDto'
import dayjs, { Dayjs } from 'dayjs'
import { useUser } from '@/hooks/useUserHook'
import { officeApi } from '@/apis/officeApi'
import {
    Autocomplete,
    Box,
    Button,
    CircularProgress,
    Grid,
    Stack,
    TextField,
    Typography,
} from '@mui/material'
import { DatePicker } from '@mui/x-date-pickers'
import { ArrowBackIos, ArrowForwardIos, Bolt } from '@mui/icons-material'
import { AxiosResponse } from 'axios'
import { SpecifikacijaNovcaTopBarButton } from '@/widgets/SpecifikacijaNovca/ui/SpecifikacijaNovcaTopBarButton'
import { SpecifikacijaNovcaDataField } from '@/widgets/SpecifikacijaNovca/ui/SpecifikacijaNovcaDataField'
import { SpecifikacijaNovcaBox } from '@/widgets/SpecifikacijaNovca/ui/SpecifikacijaNovcaBox'
import { formatNumber } from '@/helpers/numberHelpers'
import { SpecifikacijaNovcaGotovinaInputField } from '@/widgets/SpecifikacijaNovca/ui/SpecifikacijaNovcaGotovinaInputField'
import { mainTheme } from '@/themes'

export const SpecifikacijaNovca = () => {
    const [stores, setStores] = useState<IStoreDto[] | undefined>(undefined)
    const [selectedStore, setSelectedStore] = useState<IStoreDto | null>(null)
    const [date, setDate] = useState<Dayjs>(dayjs(new Date()))
    const user = useUser(false)

    const panelsSpacing = 6

    useEffect(() => {
        officeApi
            .get('/stores')
            .then((response: AxiosResponse) => setStores(response.data))
    }, [])

    const [specifikacijaNovca, setSpecifikacijaNovca] = useState<any>({
        gotovina: {
            b5000: 50,
            b2000: 0,
            b1000: 0,
            b500: 0,
            b200: 0,
            b100: 0,
            b50: 0,
            b20: 0,
            b10: 0,
            b5: 0,
            b2: 0,
            b1: 0,
        },
    })
    let specifikacijaNovcaGotovinaTotal = 0

    const handleSpecifikacijaNovcaGotovinaInputFieldChange = (
        note: number,
        value: string
    ) => {
        setSpecifikacijaNovca({
            ...specifikacijaNovca,
            gotovina: {
                ...specifikacijaNovca.gotovina,
                [`b${note}`]: value,
            },
        })
        specifikacijaNovcaGotovinaTotal = Object.keys(
            specifikacijaNovca.gotovina
        )
            .map(
                (key) =>
                    parseInt(key.substring(1)) *
                    specifikacijaNovca.gotovina[
                        `b${parseInt(key.substring(1))}`
                    ]
            )
            .reduce((acc, curr) => acc + curr)
    }

    return (
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
                                defaultValue={stores.find(
                                    (store) => store.id === user.data?.storeId
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
                            defaultValue={dayjs(new Date())}
                        />
                    </Grid>
                    <Grid item>
                        <SpecifikacijaNovcaTopBarButton text={`Osvezi`} />
                    </Grid>
                    <Grid item flexGrow={1}></Grid>
                    <Grid item>
                        <SpecifikacijaNovcaDataField
                            label={`Pretraga po broju specifikacije`}
                            defaultValue={0}
                        />
                    </Grid>
                    <Grid item>
                        <SpecifikacijaNovcaDataField
                            label={`Broj specifikacije`}
                            readonly
                            defaultValue={0}
                        />
                    </Grid>
                </Grid>
            </Grid>
            <Grid item xs={12}>
                <Grid container justifyContent={`end`}>
                    <Grid item xs={3}>
                        <SpecifikacijaNovcaTopBarButton text={`Help`} />
                    </Grid>
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
                        <Stack spacing={2}>
                            <SpecifikacijaNovcaDataField
                                readonly
                                label={`1) Gotovinski racuni:`}
                                defaultValue={-1}
                            />
                            <SpecifikacijaNovcaDataField
                                readonly
                                label={`2) Virmanski racuni:`}
                                defaultValue={-1}
                            />
                            <SpecifikacijaNovcaDataField
                                readonly
                                label={`3) Kartice:`}
                                defaultValue={-1}
                            />
                            <SpecifikacijaNovcaDataField
                                readonly
                                label={`Ukupno racunar (1+2+3):`}
                                defaultValue={-1}
                            />
                            <SpecifikacijaNovcaDataField
                                readonly
                                label={`Gotovinske povratnice:`}
                                defaultValue={-1}
                            />
                            <SpecifikacijaNovcaDataField
                                readonly
                                label={`Virmanske povratnice:`}
                                defaultValue={-1}
                            />
                            <SpecifikacijaNovcaDataField
                                readonly
                                label={`Ostale povratnice:`}
                                defaultValue={-1}
                            />
                        </Stack>
                    </SpecifikacijaNovcaBox>
                    <SpecifikacijaNovcaBox title={`Poreska`}>
                        <Stack spacing={2}>
                            <Grid container spacing={2} alignItems={`center`}>
                                <SpecifikacijaNovcaDataField
                                    readonly
                                    label={`Fiskalni racuni:`}
                                    defaultValue={-1}
                                />
                                <Grid item>
                                    <Button variant={`contained`}>
                                        <Bolt />
                                    </Button>
                                </Grid>
                            </Grid>
                            <Grid container spacing={2} alignItems={`center`}>
                                <SpecifikacijaNovcaDataField
                                    readonly
                                    label={`Virmanski racuni:`}
                                    defaultValue={-1}
                                />
                                <Grid item>
                                    <Button variant={`contained`}>
                                        <Bolt />
                                    </Button>
                                </Grid>
                            </Grid>
                        </Stack>
                    </SpecifikacijaNovcaBox>
                    <SpecifikacijaNovcaBox>
                        <Grid container>
                            <SpecifikacijaNovcaDataField multiline />
                        </Grid>
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
                                        {Object.keys(
                                            specifikacijaNovca.gotovina
                                        ).map((key, i) => {
                                            return (
                                                <SpecifikacijaNovcaGotovinaInputField
                                                    key={i}
                                                    note={parseInt(
                                                        key.substring(1)
                                                    )}
                                                    gotovinaReference={
                                                        specifikacijaNovca.gotovina
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
                                        })}
                                        <SpecifikacijaNovcaDataField
                                            label={`Ukupno gotovine:`}
                                            value={
                                                specifikacijaNovcaGotovinaTotal
                                            }
                                            readonly
                                        />
                                    </Stack>
                                </SpecifikacijaNovcaBox>
                            </Grid>
                            <Grid item>
                                <SpecifikacijaNovcaBox
                                    title={`Specifikacija Novca - Ostalo`}
                                >
                                    <Stack spacing={2}>
                                        <Grid
                                            container
                                            spacing={2}
                                            alignItems={`center`}
                                        >
                                            <SpecifikacijaNovcaDataField
                                                label={`Kartice:`}
                                                defaultValue={1}
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
                                                label={`Cekovi:`}
                                                defaultValue={1}
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
                                                label={`Papiri:`}
                                                defaultValue={1}
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
                                                label={`Troskovi:`}
                                                defaultValue={1}
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
                                                label={`Vozaci duguju:`}
                                                defaultValue={1}
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
                                                label={`Kod Sase:`}
                                                defaultValue={1}
                                            />
                                            <Grid item>
                                                <Button variant={`contained`}>
                                                    <Bolt />
                                                </Button>
                                            </Grid>
                                        </Grid>
                                    </Stack>
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
                                readonly
                                label={`Racunar trazi:`}
                                defaultValue={-1}
                            />
                        </Grid>
                        <Grid item>
                            <SpecifikacijaNovcaDataField
                                readonly
                                label={`Razlika:`}
                                defaultValue={-1}
                            />
                        </Grid>
                    </Grid>
                </SpecifikacijaNovcaBox>
            </Grid>
        </Grid>
    )
}
