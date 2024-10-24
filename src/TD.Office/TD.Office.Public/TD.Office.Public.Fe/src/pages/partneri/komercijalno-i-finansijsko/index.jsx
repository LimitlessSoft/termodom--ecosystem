import {
    Box,
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
import React, { useEffect, useState } from 'react'
import { handleApiError, officeApi } from '@/apis/officeApi'
import Grid2 from '@mui/material/Unstable_Grid2'
import { DataGrid } from '@mui/x-data-grid'
import { formatNumber } from '@/helpers/numberHelpers'
import qs from 'qs'
import {
    FINANSIJSKO,
    KOMERCIJALNO,
    TABLE_HEAD_FIELDS,
    COLUMN_TABLE_WIDTH,
    INITIAL_PAGE,
    INITIAL_PAGE_SIZE,
    PAGE_SIZE_OPTIONS,
} from '@/widgets/Partneri/PartneriFinansijskoIKomercijalno/constants'
import { ENDPOINTS } from '@/constants'

export default function KomercijalnoIFinansijsko() {
    const [data, setData] = useState(undefined)

    const [partnersRequest, setPartnersRequest] = useState({
        search: '',
        years: [],
        page: INITIAL_PAGE,
        pageSize: INITIAL_PAGE_SIZE,
    })

    const [partnersData, setPartnersData] = useState(undefined)

    const [currentFilter, setCurrentFilter] = useState(undefined)

    const [selectedYears, setSelectedYears] = useState([])

    useEffect(() => {
        officeApi
            .get(ENDPOINTS.PARTNERS.GET_KOMERCIJALNO_I_FINANSIJSKO)
            .then((res) => res.data)
            .then((data) => setData(data))
            .catch(handleApiError)
    }, [])

    const handleLoadDataButton = (e) => {
        e.preventDefault()

        if (!partnersRequest.search || partnersRequest.years.length === 0)
            return

        setPartnersData([
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
                        pocetak: 160000,
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
            {
                ppid: 113,
                naziv: 'Aomething Else',
                komercijalno: [
                    {
                        year: 2023,
                        pocetak: 50000,
                        kraj: 150000,
                    },
                ],
                finansijsko: [
                    {
                        year: 2024,
                        pocetak: 140000,
                        kraj: 250000,
                    },
                ],
            },
            {
                ppid: 114,
                naziv: 'Aomething Else',
                komercijalno: [
                    {
                        year: 2023,
                        pocetak: 50000,
                        kraj: 150000,
                    },
                ],
                finansijsko: [
                    {
                        year: 2024,
                        pocetak: 140000,
                        kraj: 250000,
                    },
                ],
            },
            {
                ppid: 115,
                naziv: 'Aomething Else',
                komercijalno: [
                    {
                        year: 2023,
                        pocetak: 50000,
                        kraj: 150000,
                    },
                ],
                finansijsko: [
                    {
                        year: 2024,
                        pocetak: 140000,
                        kraj: 250000,
                    },
                ],
            },
            {
                ppid: 116,
                naziv: 'Aomething Else',
                komercijalno: [
                    {
                        year: 2023,
                        pocetak: 50000,
                        kraj: 150000,
                    },
                ],
                finansijsko: [
                    {
                        year: 2024,
                        pocetak: 140000,
                        kraj: 250000,
                    },
                ],
            },
            {
                ppid: 117,
                naziv: 'Aomething Else',
                komercijalno: [
                    {
                        year: 2023,
                        pocetak: 50000,
                        kraj: 150000,
                    },
                ],
                finansijsko: [
                    {
                        year: 2024,
                        pocetak: 140000,
                        kraj: 250000,
                    },
                ],
            },
            {
                ppid: 118,
                naziv: 'Aomething Else',
                komercijalno: [
                    {
                        year: 2023,
                        pocetak: 50000,
                        kraj: 150000,
                    },
                ],
                finansijsko: [
                    {
                        year: 2024,
                        pocetak: 140000,
                        kraj: 250000,
                    },
                ],
            },
            {
                ppid: 119,
                naziv: 'Aomething Else',
                komercijalno: [
                    {
                        year: 2023,
                        pocetak: 50000,
                        kraj: 150000,
                    },
                ],
                finansijsko: [
                    {
                        year: 2024,
                        pocetak: 140000,
                        kraj: 250000,
                    },
                ],
            },
            {
                ppid: 120,
                naziv: 'Aomething Else',
                komercijalno: [
                    {
                        year: 2023,
                        pocetak: 50000,
                        kraj: 150000,
                    },
                ],
                finansijsko: [
                    {
                        year: 2024,
                        pocetak: 140000,
                        kraj: 250000,
                    },
                ],
            },
            {
                ppid: 121,
                naziv: 'Aomething Else',
                komercijalno: [
                    {
                        year: 2023,
                        pocetak: 50000,
                        kraj: 150000,
                    },
                ],
                finansijsko: [
                    {
                        year: 2024,
                        pocetak: 140000,
                        kraj: 250000,
                    },
                ],
            },
            {
                ppid: 122,
                naziv: 'Aomething Else',
                komercijalno: [
                    {
                        year: 2023,
                        pocetak: 50000,
                        kraj: 150000,
                    },
                ],
                finansijsko: [
                    {
                        year: 2024,
                        pocetak: 140000,
                        kraj: 250000,
                    },
                ],
            },
        ])
        setCurrentFilter(partnersRequest)

        // officeApi
        //     .get('/partneri-po-godinama-komercijalno-finansijsko-data', {
        //         params: {
        //             search: partnersRequest.search,
        //             year: partnersRequest.years,
        //             page: partnersRequest.page,
        //             pageSize: partnersRequest.pageSize,
        //         },
        //         paramsSerializer: (params) =>
        //             qs.stringify(params, { arrayFormat: 'repeat' }),
        //     })
        //     .then((res) => res.data)
        //     .then((data) => {
        //         setCurrentFilter(partnersRequest)
        //         setPartnersData(data)
        //     })
        //     .catch(handleApiError)
    }

    const renderRow = (params, year, type) => {
        const getRowData = (yearData, rowType) =>
            params.row[rowType].find(
                (row) => row.year.toString() === yearData.toString()
            )

        const currentKomercijalnoRow = getRowData(year, KOMERCIJALNO)
        const previousKomercijalnoRow = getRowData(year - 1, KOMERCIJALNO)

        const currentFinansijskoRow = getRowData(year, FINANSIJSKO)
        const previousFinansijskoRow = getRowData(year - 1, FINANSIJSKO)

        const isKomercijalnoStartGreaterThanPreviousEnd =
            previousKomercijalnoRow &&
            currentKomercijalnoRow?.pocetak - previousKomercijalnoRow?.kraj >=
                data.defaultTolerancija

        const isFinansijskoStartGreaterThanPreviousEnd =
            previousFinansijskoRow &&
            currentFinansijskoRow?.pocetak - previousFinansijskoRow?.kraj >=
                data.defaultTolerancija

        const isStart = type === TABLE_HEAD_FIELDS.POCETAK_SUFFIX

        return (
            <Stack key={year} gap={1} my={1}>
                <Typography
                    sx={{
                        color:
                            isStart && isKomercijalnoStartGreaterThanPreviousEnd
                                ? 'red'
                                : '',
                    }}
                >
                    Komercijalno:{' '}
                    {formatNumber(
                        isStart
                            ? currentKomercijalnoRow?.pocetak || 0
                            : currentKomercijalnoRow?.kraj || 0
                    )}
                </Typography>
                <Typography
                    sx={{
                        color:
                            isStart && isFinansijskoStartGreaterThanPreviousEnd
                                ? 'red'
                                : '',
                    }}
                >
                    Finansijsko:{' '}
                    {formatNumber(
                        isStart
                            ? currentFinansijskoRow?.pocetak || 0
                            : currentFinansijskoRow?.kraj || 0
                    )}
                </Typography>
            </Stack>
        )
    }

    const generateColumns = (year) => [
        {
            field: `${year}_${TABLE_HEAD_FIELDS.KRAJ_SUFFIX}`,
            headerName: `${year} - ${TABLE_HEAD_FIELDS.KRAJ_SUFFIX}`,
            width: COLUMN_TABLE_WIDTH,
            renderCell: (params) =>
                renderRow(params, year, TABLE_HEAD_FIELDS.KRAJ_SUFFIX),
        },
        {
            field: `${year}_${TABLE_HEAD_FIELDS.POCETAK_SUFFIX}`,
            headerName: `${year} - ${TABLE_HEAD_FIELDS.POCETAK_SUFFIX}`,
            width: COLUMN_TABLE_WIDTH,
            renderCell: (params) =>
                renderRow(params, year, TABLE_HEAD_FIELDS.POCETAK_SUFFIX),
        },
    ]

    const renderCell = (params) => (
        <Box>
            <Typography>{params.value}</Typography>
        </Box>
    )

    console.log(partnersRequest)

    return (
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
                                    renderValue={(selected) =>
                                        selected.join(', ')
                                    }
                                    onChange={(e) => {
                                        const uniqueYears = [
                                            ...new Set(e.target.value.flat()),
                                        ]

                                        setPartnersRequest((prev) => ({
                                            ...prev,
                                            years: uniqueYears,
                                        }))
                                    }}
                                    value={partnersRequest.years ?? []}
                                    sx={{ width: 300 }}
                                >
                                    {data.years.map((year) => (
                                        <MenuItem
                                            key={year.value}
                                            value={year.key}
                                        >
                                            <Checkbox
                                                checked={partnersRequest.years?.includes(
                                                    year.key
                                                )}
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
                                onClick={handleLoadDataButton}
                            >
                                Učitaj
                            </Button>
                        </Grid2>
                    </Grid2>
                </Paper>
            ) : (
                <CircularProgress />
            )}

            {partnersData && partnersData.length > 0 && (
                <Paper>
                    <Stack gap={2} m={2}>
                        <Stack direction={`row`} justifyContent={`flex-end`}>
                            <TextField
                                defaultValue={data.defaultTolerancija}
                                label={`Tolerancija`}
                                sx={{ maxWidth: 200 }}
                                onChange={(e) =>
                                    setData((prev) => ({
                                        ...prev,
                                        defaultTolerancija: e.target.value,
                                    }))
                                }
                            />
                        </Stack>

                        <DataGrid
                            getRowId={(row) => row.ppid}
                            pageSizeOptions={PAGE_SIZE_OPTIONS}
                            paginationModel={{
                                page: partnersRequest.page,
                                pageSize: partnersRequest.pageSize,
                            }}
                            onPaginationModelChange={(paginationData) => {
                                setPartnersRequest((prev) => ({
                                    ...prev,
                                    ...paginationData,
                                }))
                            }}
                            columns={[
                                {
                                    field: TABLE_HEAD_FIELDS.PPID.toLowerCase(),
                                    headerName: TABLE_HEAD_FIELDS.PPID,
                                    width: 150,
                                    pinnable: true,
                                    headerClassName: 'sticky-header',
                                    renderCell,
                                },
                                {
                                    field: TABLE_HEAD_FIELDS.NAZIV.toLowerCase(),
                                    headerName: TABLE_HEAD_FIELDS.NAZIV,
                                    width: 150,
                                    pinnable: true,
                                    headerClassName: 'sticky-header',
                                    renderCell,
                                },
                                ...currentFilter.years
                                    .toSorted((a, b) => b - a)
                                    .map((year) => generateColumns(year))
                                    .flat(),
                            ]}
                            rows={partnersData}
                            initialState={{
                                pagination: {
                                    paginationModel: {
                                        pageSize: INITIAL_PAGE_SIZE,
                                    },
                                },
                                pinnedColumns: {
                                    left: [
                                        TABLE_HEAD_FIELDS.PPID.toLowerCase(),
                                        TABLE_HEAD_FIELDS.NAZIV.toLowerCase(),
                                    ],
                                },
                            }}
                            sx={{
                                position: 'relative',
                                '& .sticky-header': {
                                    position: 'sticky',
                                    top: 0,
                                    left: 0,
                                    zIndex: 2,
                                    background: 'white',
                                },
                                '& .MuiDataGrid-cell': {
                                    padding: '10px',
                                },
                            }}
                            getRowHeight={(params) => 'auto'}
                        />
                    </Stack>
                </Paper>
            )}
        </Stack>
    )
}
