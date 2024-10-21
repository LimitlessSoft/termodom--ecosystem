import {
    Button,
    FormControl,
    InputLabel,
    Paper,
    Select,
    Stack,
    TextField,
    Checkbox,
    Typography,
    CircularProgress,
    MenuItem,
} from '@mui/material'
import React, { useEffect, useMemo, useState } from 'react'
import SubModuledLayout from '@/widgets/SubModuledLayout/ui/SubModuledLayout'
import { handleApiError, officeApi } from '@/apis/officeApi'
import Grid2 from '@mui/material/Unstable_Grid2'
import { toast } from 'react-toastify'
import { DataGrid } from '@mui/x-data-grid'
import { formatNumber } from '../../helpers/numberHelpers'
import { usePartneriSubModules } from '../../subModules/usePartneriSubModules'

const Partneri = () => {
    const [data, setData] = useState(undefined)

    const dataColumnWidth = 300

    const [partnersRequest, setPartnersRequest] = useState({})
    const [partnersData, setPartnersData] = useState([
        {
            ppid: 111,
            naziv: 'Something',
            komercijalno: [
                {
                    year: 2022,
                    pocetak: 50000,
                    kraj: 150000,
                },
                {
                    year: 2023,
                    pocetak: 50000,
                    kraj: 150000,
                },
                {
                    year: 2024,
                    pocetak: 75000,
                    kraj: 200000,
                },
            ],
            finansijsko: [
                {
                    year: 2022,
                    pocetak: 50000,
                    kraj: 150000,
                },
                {
                    year: 2023,
                    pocetak: 50000,
                    kraj: 150000,
                },
                {
                    year: 2024,
                    pocetak: 100000,
                    kraj: 200000,
                },
            ],
        },
        {
            ppid: 112,
            naziv: 'Aomething Else',
            komercijalno: [
                {
                    year: 2022,
                    pocetak: 50000,
                    kraj: 150000,
                },
                {
                    year: 2023,
                    pocetak: 50000,
                    kraj: 150000,
                },
                {
                    year: 2024,
                    pocetak: 60000,
                    kraj: 180000,
                },
            ],
            finansijsko: [
                {
                    year: 2022,
                    pocetak: 50000,
                    kraj: 150000,
                },
                {
                    year: 2023,
                    pocetak: 30000,
                    kraj: 120000,
                },
                {
                    year: 2024,
                    pocetak: 140000,
                    kraj: 250000,
                },
            ],
        },
    ])

    useEffect(() => {
        officeApi
            .get('/partneri-po-godinama-komercijalno-finansijsko')
            .then((res) => res.data)
            .then((data) => setData(data))
            .catch(handleApiError)
    }, [])

    // useEffect(() => {
    //     officeApi
    //         .get('/partneri-po-godinama-komercijalno-finansijsko-data', {
    //             params: {
    //                 search: currentFilter.search,
    //                 year: currentFilter.years,
    //             },
    //             paramsSerializer: (params) =>
    //                 qs.stringify(params, { arrayFormat: 'repeat' }),
    //         })
    //         .then((res) => res.data)
    //         .then((data) => setPartnersData(data))
    //         .catch(handleApiError)
    // }, [currentFilter.search, currentFilter.years])

    const subModules = usePartneriSubModules()
    return (
        <SubModuledLayout modules={subModules}>
            <Stack gap={2}>
                {data ? (
                    <Paper
                        sx={{
                            maxWidth: 400,
                            p: 2,
                        }}
                    >
                        <Grid2 container alignItems={`center`} gap={2}>
                            <Grid2>
                                <FormControl>
                                    <InputLabel>Godine</InputLabel>
                                    <Select
                                        multiple
                                        label="Godine"
                                        variant="outlined"
                                        renderValue={(selected) => {
                                            return selected.join(', ')
                                        }}
                                        onChange={(e) => {
                                            setPartnersRequest({
                                                ...partnersRequest,
                                                years: e.target.value,
                                            })
                                        }}
                                        value={partnersRequest.years ?? []}
                                        sx={{
                                            width: 300,
                                        }}
                                    >
                                        {data.years.map((year) => (
                                            <MenuItem
                                                key={year.value}
                                                value={year.key}
                                            >
                                                <Checkbox
                                                    checked={
                                                        partnersRequest?.years?.includes(
                                                            year.value
                                                        ) ?? false
                                                    }
                                                />
                                                {year.key}
                                            </MenuItem>
                                        ))}
                                    </Select>
                                </FormControl>
                            </Grid2>
                            <Grid2>
                                <TextField
                                    label="Pretraga"
                                    variant="outlined"
                                    onChange={(e) => {
                                        setPartnersRequest({
                                            ...partnersRequest,
                                            search: e.target.value,
                                        })
                                    }}
                                />
                            </Grid2>
                            <Grid2>
                                <Button
                                    variant="contained"
                                    onClick={() => {
                                        toast(JSON.stringify(partnersRequest))
                                    }}
                                >
                                    Učitaj
                                </Button>
                            </Grid2>
                        </Grid2>
                    </Paper>
                ) : (
                    <CircularProgress />
                )}
                {partnersData &&
                    data &&
                    partnersRequest &&
                    partnersRequest.years && (
                        <Paper>
                            <Stack gap={2} m={2}>
                                <Stack
                                    direction={`row`}
                                    justifyContent={`flex-end`}
                                >
                                    <TextField
                                        defaultValue={data.defaultTolerancija}
                                        label={`Tolerancija`}
                                        sx={{
                                            maxWidth: 200,
                                        }}
                                    />
                                </Stack>
                                <DataGrid
                                    getRowId={(row) => row.ppid}
                                    columns={[
                                        {
                                            field: 'ppid',
                                            headerName: 'PPID',
                                            width: 150,
                                            pinnable: true,
                                        },
                                        {
                                            field: 'naziv',
                                            headerName: 'Naziv',
                                            width: 150,
                                            pinnable: true,
                                        },
                                        ...partnersRequest.years
                                            .toSorted((y) => -y)
                                            .map((year) => [
                                                {
                                                    field: `${year}_kraj`,
                                                    headerName: `${year} - Kraj`,
                                                    width: dataColumnWidth,
                                                    renderCell: (params) => {
                                                        return (
                                                            <Stack
                                                                gap={1}
                                                                my={1}
                                                            >
                                                                <Typography>
                                                                    Komercijalno:
                                                                    {formatNumber(
                                                                        params.row[
                                                                            'komercijalno'
                                                                        ].find(
                                                                            (
                                                                                x
                                                                            ) =>
                                                                                x.year.toString() ===
                                                                                year.toString()
                                                                        ).kraj
                                                                    )}
                                                                </Typography>
                                                                <Typography>
                                                                    Finansijsko:
                                                                    {formatNumber(
                                                                        params.row[
                                                                            'finansijsko'
                                                                        ].find(
                                                                            (
                                                                                x
                                                                            ) =>
                                                                                x.year.toString() ===
                                                                                year.toString()
                                                                        ).kraj
                                                                    )}
                                                                </Typography>
                                                            </Stack>
                                                        )
                                                    },
                                                },
                                                {
                                                    field: `${year}_pocetak`,
                                                    headerName: `${year} - Pocetak`,
                                                    width: dataColumnWidth,
                                                    renderCell: (params) => {
                                                        return (
                                                            <Stack
                                                                gap={1}
                                                                my={1}
                                                            >
                                                                <Typography>
                                                                    Komercijalno:
                                                                    {formatNumber(
                                                                        params.row[
                                                                            'komercijalno'
                                                                        ].find(
                                                                            (
                                                                                x
                                                                            ) =>
                                                                                x.year.toString() ===
                                                                                year.toString()
                                                                        )
                                                                            .pocetak
                                                                    )}
                                                                </Typography>
                                                                <Typography>
                                                                    Finansijsko:
                                                                    {formatNumber(
                                                                        params.row[
                                                                            'finansijsko'
                                                                        ].find(
                                                                            (
                                                                                x
                                                                            ) =>
                                                                                x.year.toString() ===
                                                                                year.toString()
                                                                        )
                                                                            .pocetak
                                                                    )}
                                                                </Typography>
                                                            </Stack>
                                                        )
                                                    },
                                                },
                                            ])
                                            .flat(),
                                    ]}
                                    rows={partnersData}
                                    initialState={{
                                        pinnedColumns: {
                                            left: ['ppid', 'naziv'],
                                        },
                                    }}
                                    getRowHeight={(params) => 'auto'}
                                />
                            </Stack>
                        </Paper>
                    )}
                {/*<TableContainer component={Paper}>*/}
                {/*    <Table>*/}
                {/*        <TableHead>*/}
                {/*            <TableRow>*/}
                {/*                <TableCell*/}
                {/*                    align={ALIGNMENT}*/}
                {/*                    onClick={() =>*/}
                {/*                        sortData(TABLE_HEAD_FIELDS.PPID)*/}
                {/*                    }*/}
                {/*                >*/}
                {/*                    <Grid*/}
                {/*                        container*/}
                {/*                        alignItems={ALIGNMENT}*/}
                {/*                        justifyContent={ALIGNMENT}*/}
                {/*                    >*/}
                {/*                        <Grid item>*/}
                {/*                            <Typography>*/}
                {/*                                {TABLE_HEAD_FIELDS.PPID}*/}
                {/*                            </Typography>*/}
                {/*                        </Grid>*/}
                {/*                        <Grid item>*/}
                {/*                            <SwapVert />*/}
                {/*                        </Grid>*/}
                {/*                    </Grid>*/}
                {/*                </TableCell>*/}
                {/*                <TableCell*/}
                {/*                    align={ALIGNMENT}*/}
                {/*                    onClick={() =>*/}
                {/*                        sortData(TABLE_HEAD_FIELDS.NAZIV)*/}
                {/*                    }*/}
                {/*                >*/}
                {/*                    <Grid*/}
                {/*                        container*/}
                {/*                        alignItems={ALIGNMENT}*/}
                {/*                        justifyContent={ALIGNMENT}*/}
                {/*                    >*/}
                {/*                        <Grid item>*/}
                {/*                            <Typography>*/}
                {/*                                {TABLE_HEAD_FIELDS.NAZIV}*/}
                {/*                            </Typography>*/}
                {/*                        </Grid>*/}
                {/*                        <Grid item>*/}
                {/*                            <SwapVert />*/}
                {/*                        </Grid>*/}
                {/*                    </Grid>*/}
                {/*                </TableCell>*/}
                {/*                {defaultData.years.map((year) => (*/}
                {/*                    <React.Fragment key={year.value}>*/}
                {/*                        <TableCell align={ALIGNMENT}>*/}
                {/*                            {`${formatYear(year.key)}_${TABLE_HEAD_FIELDS.POCETAK_SUFFIX}`}*/}
                {/*                        </TableCell>*/}
                {/*                        <TableCell align={ALIGNMENT}>*/}
                {/*                            {`${formatYear(year.key)}_${TABLE_HEAD_FIELDS.KRAJ_SUFFIX}`}*/}
                {/*                        </TableCell>*/}
                {/*                    </React.Fragment>*/}
                {/*                ))}*/}
                {/*            </TableRow>*/}
                {/*        </TableHead>*/}
                {/*        <TableBody>*/}
                {/*            {partnersData.map((partner) => (*/}
                {/*                <React.Fragment key={partner.ppid}>*/}
                {/*                    <PartneriFinansijskoIKomercijalnoYearRow*/}
                {/*                        defaultData={defaultData}*/}
                {/*                        partner={partner}*/}
                {/*                        type={KOMERCIJALNO}*/}
                {/*                    />*/}
                {/*                    <PartneriFinansijskoIKomercijalnoYearRow*/}
                {/*                        defaultData={defaultData}*/}
                {/*                        partner={partner}*/}
                {/*                        type={FINANSIJSKO}*/}
                {/*                    />*/}
                {/*                </React.Fragment>*/}
                {/*            ))}*/}
                {/*        </TableBody>*/}
                {/*    </Table>*/}
                {/*</TableContainer>*/}
            </Stack>
        </SubModuledLayout>
    )
}

export default Partneri
