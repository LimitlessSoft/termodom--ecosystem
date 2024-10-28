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
    ENDPOINTS_CONSTANTS,
    PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS,
} from '@/constants'

export default function KomercijalnoIFinansijsko() {
    const [data, setData] = useState({
        years: [
            { key: '2024', value: 'TCMDZ 2024' },
            { key: '2023', value: 'TCMDZ 2023' },
            { key: '2022', value: 'TCMDZ 2022' },
            { key: '2021', value: 'TCMDZ 2021' },
            { key: '2020', value: 'TCMDZ 2020' },
        ],
        defaultTolerancija: 20001,
    })

    const [partnersRequest, setPartnersRequest] = useState({
        search: '',
        years: [],
        page: PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS.INITIAL_PAGE,
        pageSize:
            PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS.INITIAL_PAGE_SIZE,
    })

    const [partnersData, setPartnersData] = useState(undefined)
    const [pagination, setPagination] = useState({
        pageSize:
            PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS.INITIAL_PAGE_SIZE,
        page: PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS.INITIAL_PAGE,
    })

    // useEffect(() => {
    //     officeApi
    //         .get(ENDPOINTS_CONSTANTS.PARTNERS.GET_KOMERCIJALNO_I_FINANSIJSKO)
    //         .then((res) => res.data)
    //         .then((data) => setData(data))
    //         .catch(handleApiError)
    // }, [])

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
                        kraj: 160000,
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
                        kraj: 140000,
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

        //  getPartnersData()
    }

    useEffect(() => {
        getPartnersData()
    }, [pagination])

    const getPartnersData = () => {
        officeApi
            .get('/partneri-po-godinama-komercijalno-finansijsko-data', {
                params: {
                    search: partnersRequest.search,
                    year: partnersRequest.years,
                    currentPage: pagination.page + 1,
                    pageSize: partnersRequest.pageSize,
                },
                paramsSerializer: (params) =>
                    qs.stringify(params, { arrayFormat: 'repeat' }),
            })
            .then((res) => res.data)
            .then((data) => setPartnersData(data))
            .catch(handleApiError)
    }

    console.log(data.defaultTolerancija)

    const renderRow = (params, year, type) => {
        const { KOMERCIJALNO, FINANSIJSKO } =
            PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS

        const getRowData = (yearData, rowType) =>
            params.row[rowType].find(
                (row) => row.year.toString() === yearData.toString()
            )

        const currentKomercijalnoRow = getRowData(year, KOMERCIJALNO)
        const previousKomercijalnoRow = getRowData(year - 1, KOMERCIJALNO)

        const currentFinansijskoRow = getRowData(year, FINANSIJSKO)
        const previousFinansijskoRow = getRowData(year - 1, FINANSIJSKO)

        const isToleranceExceeded = (val1, val2) =>
            Math.abs((val1 || 0) - (val2 || 0)) >= data.defaultTolerancija

        const isKomercijalnoStartGreaterThanPreviousEnd = isToleranceExceeded(
            currentKomercijalnoRow?.pocetak,
            previousKomercijalnoRow?.kraj
        )

        const isFinansijskoStartGreaterThanPreviousEnd = isToleranceExceeded(
            currentFinansijskoRow?.pocetak,
            previousFinansijskoRow?.kraj
        )

        const isDifferenceBetweenPocetakExceeded = isToleranceExceeded(
            currentKomercijalnoRow?.pocetak,
            currentFinansijskoRow?.pocetak
        )

        const isDifferenceBetweenKrajExceeded = isToleranceExceeded(
            currentKomercijalnoRow?.kraj,
            currentFinansijskoRow?.kraj
        )

        const isStart =
            type ===
            PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS.TABLE_HEAD_FIELDS
                .POCETAK_SUFFIX
        const isEnd =
            type ===
            PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS.TABLE_HEAD_FIELDS
                .KRAJ_SUFFIX

        // console.log(
        //     (currentKomercijalnoRow?.pocetak || 0) -
        //         (currentFinansijskoRow?.pocetak || 0)
        // )

        const stackColor =
            (isStart && isDifferenceBetweenPocetakExceeded) ||
            (isEnd && isDifferenceBetweenKrajExceeded)
                ? 'red'
                : ''

        return (
            <Stack key={year} gap={1} my={1} color={stackColor}>
                <Typography
                    sx={{
                        color:
                            isStart &&
                            (isKomercijalnoStartGreaterThanPreviousEnd ||
                                isDifferenceBetweenPocetakExceeded)
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
            field: `${year}_${PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS.TABLE_HEAD_FIELDS.KRAJ_SUFFIX}`,
            headerName: `${year} - ${PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS.TABLE_HEAD_FIELDS.KRAJ_SUFFIX}`,
            width: PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS.COLUMN_TABLE_WIDTH,
            renderCell: (params) =>
                renderRow(
                    params,
                    year,
                    PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS
                        .TABLE_HEAD_FIELDS.KRAJ_SUFFIX
                ),
        },
        {
            field: `${year}_${PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS.TABLE_HEAD_FIELDS.POCETAK_SUFFIX}`,
            headerName: `${year} - ${PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS.TABLE_HEAD_FIELDS.POCETAK_SUFFIX}`,
            width: PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS.COLUMN_TABLE_WIDTH,
            renderCell: (params) =>
                renderRow(
                    params,
                    year,
                    PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS
                        .TABLE_HEAD_FIELDS.POCETAK_SUFFIX
                ),
        },
    ]

    const renderCell = (params) => (
        <Box>
            <Typography>{params.value}</Typography>
        </Box>
    )

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
                                        if (partnersData)
                                            setPartnersData(undefined)

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
                            pageSizeOptions={
                                PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS.PAGE_SIZE_OPTIONS
                            }
                            paginationModel={pagination}
                            onPaginationModelChange={setPagination}
                            columns={[
                                {
                                    field: PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS.TABLE_HEAD_FIELDS.PPID.toLowerCase(),
                                    headerName:
                                        PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS
                                            .TABLE_HEAD_FIELDS.PPID,
                                    width: 150,
                                    pinnable: true,
                                    headerClassName: 'sticky-header',
                                    renderCell,
                                },
                                {
                                    field: PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS.TABLE_HEAD_FIELDS.NAZIV.toLowerCase(),
                                    headerName:
                                        PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS
                                            .TABLE_HEAD_FIELDS.NAZIV,
                                    width: 150,
                                    pinnable: true,
                                    headerClassName: 'sticky-header',
                                    renderCell,
                                },
                                ...partnersRequest.years
                                    .toSorted((a, b) => b - a)
                                    .map((year) => generateColumns(year))
                                    .flat(),
                            ]}
                            rows={partnersData}
                            initialState={{
                                pagination: {
                                    paginationModel: pagination,
                                },
                            }}
                            checkboxSelection={false}
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
