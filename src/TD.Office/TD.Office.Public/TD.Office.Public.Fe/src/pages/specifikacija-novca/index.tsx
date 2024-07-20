import { officeApi } from '@/apis/officeApi'
import { useUser } from '@/hooks/useUserHook'
import { ArrowBackIos, ArrowForwardIos } from '@mui/icons-material'
import {
    Autocomplete,
    Box,
    Button,
    Grid,
    TextField,
    Stack,
    Typography,
} from '@mui/material'
import { DatePicker } from '@mui/x-date-pickers'
import dayjs, { Dayjs } from 'dayjs'
import { useEffect, useState } from 'react'

interface Store {
    id: number
    name: string
}

const SpecifikacijNovca = (): JSX.Element => {
    const [stores, setStores] = useState<Store[] | undefined>(undefined)
    const [selectedStore, setSelectedStore] = useState<Store | null>(null)
    const [date, setDate] = useState<Dayjs>(dayjs(new Date()))
    const user = useUser(false)

    useEffect(() => {
        officeApi
            .get('/stores')
            .then((response: any) => setStores(response.data))
    }, [])

    return (
        <Grid container padding={4} spacing={2}>
            <Grid item xs={12}>
                <Grid container spacing={2}>
                    <Grid item xs={4}>
                        {stores && stores.length > 0 && (
                            <Autocomplete
                                defaultValue={stores!.find(
                                    (store) => store.id === user.data?.storeId
                                )}
                                options={stores!}
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
                            label="Datum"
                            onChange={(newDate) => newDate && setDate(newDate)}
                            defaultValue={dayjs(new Date())}
                        />
                    </Grid>
                    <Grid item>
                        <Button variant={`outlined`}>Osvezi</Button>
                    </Grid>
                    <Grid item flexGrow={1}></Grid>
                    <Grid item>
                        <TextField
                            variant={`outlined`}
                            value={0}
                            label={`Pretraga po broju specifikacije`}
                        />
                    </Grid>
                    <Grid item>
                        <TextField
                            variant={`outlined`}
                            value={0}
                            disabled
                            label={`Broj specifikacije`}
                        />
                    </Grid>
                </Grid>
            </Grid>
            <Grid item xs={12}>
                <Grid container justifyContent={`end`}>
                    <Grid item xs={3}>
                        <Button variant={`outlined`}>
                            <Typography>Help</Typography>
                        </Button>
                    </Grid>
                    <Grid item>
                        <Grid container spacing={1}>
                            <Grid item>
                                <Button variant={`outlined`}>
                                    <ArrowBackIos
                                        style={{ transform: 'translateX(4px)' }}
                                    />
                                </Button>
                            </Grid>
                            <Grid item>
                                <Button variant={`outlined`}>
                                    <Typography fontWeight={`bold`}>
                                        M
                                    </Typography>
                                </Button>
                            </Grid>
                            <Grid item>
                                <Button variant={`outlined`}>
                                    <ArrowForwardIos />
                                </Button>
                            </Grid>
                        </Grid>
                    </Grid>
                </Grid>
            </Grid>
            <Grid item>
                <Grid container direction={`column`}>
                    <Grid item>
                        <Grid container>
                            <Grid item>
                                <Stack spacing={2}>
                                    <Grid item alignItems={`center`}>
                                        <TextField
                                            variant={`outlined`}
                                            disabled
                                            value={123}
                                            label={`1) Gotovinski racuni:`}
                                        />
                                    </Grid>
                                    <Grid item alignItems={`center`}>
                                        <TextField
                                            variant={`outlined`}
                                            disabled
                                            value={123}
                                            label={`2) Virmanski racuni:`}
                                        />
                                    </Grid>
                                    <Grid item alignItems={`center`}>
                                        <TextField
                                            variant={`outlined`}
                                            disabled
                                            value={123}
                                            label={`3) Kartice:`}
                                        />
                                    </Grid>
                                    <Grid item alignItems={`center`}>
                                        <TextField
                                            variant={`outlined`}
                                            disabled
                                            value={123}
                                            label={`Ukupno racunar (1+2+3):`}
                                        />
                                    </Grid>
                                    <Grid item alignItems={`center`}>
                                        <TextField
                                            variant={`outlined`}
                                            disabled
                                            value={123}
                                            label={`Gotovinske povratnice:`}
                                        />
                                    </Grid>
                                    <Grid item alignItems={`center`}>
                                        <TextField
                                            variant={`outlined`}
                                            disabled
                                            value={123}
                                            label={`Virmanske povratnice:`}
                                        />
                                    </Grid>
                                    <Grid item alignItems={`center`}>
                                        <TextField
                                            variant={`outlined`}
                                            disabled
                                            value={123}
                                            label={`Ostale povratnice:`}
                                        />
                                    </Grid>
                                </Stack>
                            </Grid>
                            <Grid item>
                                <Stack>Poreska</Stack>
                            </Grid>
                        </Grid>
                    </Grid>
                    <Grid item>
                        <Grid container>asd</Grid>
                    </Grid>
                </Grid>
                {/* <Grid container spacing={2}>
                    <Grid item padding={4}>
                        <Grid container direction={`column`}>
                            <Grid item>
                                <Stack>
                                    <Grid item alignItems={`center`}>
                                        <Typography>
                                            1) Gotovinski racuni:
                                        </Typography>
                                        <TextField
                                            variant={`outlined`}
                                            disabled
                                            value={123}
                                        />
                                    </Grid>
                                    <Grid item alignItems={`center`}>
                                        <Typography>
                                            2) Virmanski racuni:
                                        </Typography>
                                        <TextField
                                            variant={`outlined`}
                                            disabled
                                            value={123}
                                        />
                                    </Grid>
                                    <Grid item alignItems={`center`}>
                                        <Typography>3) Kartice:</Typography>
                                        <TextField
                                            variant={`outlined`}
                                            disabled
                                            value={123}
                                        />
                                    </Grid>
                                    <Grid item alignItems={`center`}>
                                        <Typography>
                                            Ukupno racunar (1+2+3):
                                        </Typography>
                                        <TextField
                                            variant={`outlined`}
                                            disabled
                                            value={123}
                                        />
                                    </Grid>
                                </Stack>
                            </Grid>
                            <Grid item flexGrow={1}></Grid>
                            <Grid item>
                                <Stack>
                                    <Grid item alignItems={`center`}>
                                        <Typography>
                                            Gotovinske povratnice
                                        </Typography>
                                        <TextField
                                            variant={`outlined`}
                                            disabled
                                            value={123}
                                        />
                                    </Grid>
                                    <Grid item alignItems={`center`}>
                                        <Typography>
                                            Virmanske povratnice
                                        </Typography>
                                        <TextField
                                            variant={`outlined`}
                                            disabled
                                            value={123}
                                        />
                                    </Grid>
                                    <Grid item alignItems={`center`}>
                                        <Typography>
                                            Ostale povratnice
                                        </Typography>
                                        <TextField
                                            variant={`outlined`}
                                            disabled
                                            value={123}
                                        />
                                    </Grid>
                                </Stack>
                            </Grid>
                        </Grid>
                    </Grid>
                    <Grid item>
                        <Grid alignItems={`center`}>
                            <TextField variant={`outlined`} />
                            <Typography>eur x</Typography>
                            <TextField variant={`outlined`} />
                            <Typography>=</Typography>
                            <TextField variant={`outlined`} />
                        </Grid>
                        <Grid alignItems={`center`}>
                            <TextField variant={`outlined`} />
                            <Typography>eur x</Typography>
                            <TextField variant={`outlined`} />
                            <Typography>=</Typography>
                            <TextField variant={`outlined`} />
                        </Grid>
                        <Grid alignItems={`center`} textAlign={`end`}>
                            <Typography>5000 x</Typography>
                            <TextField variant={`outlined`} />
                            <Typography>=</Typography>
                            <TextField variant={`outlined`} />
                        </Grid>
                        <Grid alignItems={`center`}>
                            <Typography>2000 x</Typography>
                            <TextField variant={`outlined`} />
                            <Typography>=</Typography>
                            <TextField variant={`outlined`} />
                        </Grid>
                        <Grid alignItems={`center`}>
                            <Typography>1000 x</Typography>
                            <TextField variant={`outlined`} />
                            <Typography>=</Typography>
                            <TextField variant={`outlined`} />
                        </Grid>
                        <Grid alignItems={`center`}>
                            <Typography>500 x</Typography>
                            <TextField variant={`outlined`} />
                            <Typography>=</Typography>
                            <TextField variant={`outlined`} />
                        </Grid>
                        <Grid alignItems={`center`}>
                            <Typography>200 x</Typography>
                            <TextField variant={`outlined`} />
                            <Typography>=</Typography>
                            <TextField variant={`outlined`} />
                        </Grid>
                        <Grid alignItems={`center`}>
                            <Typography>100 x</Typography>
                            <TextField variant={`outlined`} />
                            <Typography>=</Typography>
                            <TextField variant={`outlined`} />
                        </Grid>
                        <Grid alignItems={`center`}>
                            <Typography>50 x</Typography>
                            <TextField variant={`outlined`} />
                            <Typography>=</Typography>
                            <TextField variant={`outlined`} />
                        </Grid>
                        <Grid alignItems={`center`}>
                            <Typography>20 x</Typography>
                            <TextField variant={`outlined`} />
                            <Typography>=</Typography>
                            <TextField variant={`outlined`} />
                        </Grid>
                        <Grid alignItems={`center`}>
                            <Typography>10 x</Typography>
                            <TextField variant={`outlined`} />
                            <Typography>=</Typography>
                            <TextField variant={`outlined`} />
                        </Grid>
                        <Grid alignItems={`center`}>
                            <Typography>5 x</Typography>
                            <TextField variant={`outlined`} />
                            <Typography>=</Typography>
                            <TextField variant={`outlined`} />
                        </Grid>
                        <Grid alignItems={`center`}>
                            <Typography>2 x</Typography>
                            <TextField variant={`outlined`} />
                            <Typography>=</Typography>
                            <TextField variant={`outlined`} />
                        </Grid>
                        <Grid alignItems={`center`} textAlign={`end`}>
                            <Typography>1 x</Typography>
                            <TextField variant={`outlined`} />
                            <Typography>=</Typography>
                            <TextField variant={`outlined`} />
                        </Grid>
                    </Grid>
                </Grid> */}
            </Grid>
            <Grid item>
                <Grid container>asd</Grid>
            </Grid>
        </Grid>
    )
}

export default SpecifikacijNovca
