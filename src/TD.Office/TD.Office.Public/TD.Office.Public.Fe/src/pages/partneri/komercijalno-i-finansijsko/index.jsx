import {
    Button,
    Paper,
    Stack,
    TextField,
    CircularProgress,
    Pagination,
    Box,
    Typography,
    ToggleButton,
    Switch,
} from '@mui/material'
import React, { useEffect, useState } from 'react'
import { handleApiError, officeApi } from '@/apis/officeApi'
import Grid2 from '@mui/material/Unstable_Grid2'
import qs from 'qs'
import {
    ENDPOINTS_CONSTANTS,
    PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS,
} from '@/constants'
import { ComboBoxInput, PartneriKomercijalnoIFinansijskoTable } from '@/widgets'

export default function PartneriKomercijalnoIFinansijsko() {
    const [data, setData] = useState(undefined)
    const [isDataLoading, setIsDataLoading] = useState(false)
    const [statuses, setStatuses] = useState(undefined)

    const [partnersData, setPartnersData] = useState(undefined)
    const [isPartnersDataLoading, setIsPartnersDataLoading] = useState(false)

    const [partnersRequest, setPartnersRequest] = useState({
        search: '',
        years: [],
        tolerancija: 0,
    })
    const [pagination, setPagination] = useState({
        pageSize:
            PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS.INITIAL_PAGE_SIZE,
        page: PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS.INITIAL_PAGE,
    })

    const [continousLoad, setContinousLoad] = useState(true)

    useEffect(() => {
        getPartnersData()
    }, [pagination])

    useEffect(() => {
        setIsDataLoading(true)
        officeApi
            .get(ENDPOINTS_CONSTANTS.PARTNERS.GET_KOMERCIJALNO_I_FINANSIJSKO)
            .then((res) => res.data)
            .then((data) => {
                setData(data)
                setPartnersRequest((prev) => ({
                    ...prev,
                    tolerancija: data.defaultTolerancija,
                }))
            })
            .catch(handleApiError)
            .finally(() => setIsDataLoading(false))

        // fetch statuses here
        setStatuses([
            {
                id: 1,
                name: 'Mocked 1',
            },
            {
                id: 2,
                name: 'Mocked 2',
            },
            {
                id: 3,
                name: 'Mocked 3',
            },
        ])
    }, [])

    const getPartnersData = () => {
        setIsPartnersDataLoading(true)
        officeApi
            .get(
                ENDPOINTS_CONSTANTS.PARTNERS
                    .GET_KOMERCIJALNO_I_FINANSIJSKO_DATA,
                {
                    params: {
                        searchKeyword: partnersRequest.search,
                        tolerancija: partnersRequest.tolerancija,
                        years: partnersRequest.years,
                        currentPage: pagination.page,
                        pageSize: pagination.pageSize,
                    },
                    paramsSerializer: (params) =>
                        qs.stringify(params, { arrayFormat: 'repeat' }),
                }
            )
            .then((res) => res.data)
            .then((data) => {
                if (continousLoad) {
                    setPartnersData((prev) => ({
                        payload: prev?.payload
                            ? [...prev.payload, ...data.payload]
                            : data.payload,
                        pagination: data.pagination,
                    }))

                    if (data.pagination.totalPages > pagination.page) {
                        setPagination((prev) => ({
                            ...prev,
                            page: prev.page + 1,
                        }))
                    }
                } else {
                    setPartnersData(data)
                }
            })
            .catch(handleApiError)
            .finally(() => setIsPartnersDataLoading(false))
    }

    const handleLoadDataButton = (e) => {
        e.preventDefault()

        if (partnersRequest.years.length === 0) return

        setPagination((prev) => ({
            ...prev,
            page: PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS.INITIAL_PAGE,
        })) // This will trigger reload
    }

    const onSelectionChange = (e) => {
        if (partnersData) setPartnersData(undefined)

        const uniqueYears = [...new Set(e.target.value.flat())]

        setPartnersRequest((prev) => ({
            ...prev,
            years: uniqueYears,
        }))
    }

    const handleSearchChange = (e) => {
        setPartnersRequest({
            ...partnersRequest,
            search: e.target.value,
        })

        if (partnersData) setPartnersData(undefined)
    }

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
                            <ComboBoxInput
                                label={`Godine`}
                                onSelectionChange={onSelectionChange}
                                selectedValues={partnersRequest.years}
                                options={data.years}
                                style={{ width: 300 }}
                                disabled={
                                    isDataLoading || isPartnersDataLoading
                                }
                            />
                        </Grid2>

                        <Grid2>
                            <TextField
                                label="Pretraga"
                                variant="outlined"
                                onChange={handleSearchChange}
                                disabled={
                                    isDataLoading || isPartnersDataLoading
                                }
                            />
                        </Grid2>

                        <Grid2>
                            <TextField
                                defaultValue={data.defaultTolerancija}
                                label={`Tolerancija`}
                                sx={{ maxWidth: 200 }}
                                onChange={(e) => {
                                    setPartnersRequest((prev) => ({
                                        ...prev,
                                        tolerancija: e.target.value,
                                    }))

                                    setData((prev) => ({
                                        ...prev,
                                        defaultTolerancija: e.target.value,
                                    }))
                                }}
                                disabled={
                                    isDataLoading || isPartnersDataLoading
                                }
                            />
                        </Grid2>

                        <Grid2>
                            <Typography variant={`span`}>
                                Kontinualno učitavanje podataka
                            </Typography>
                            <Switch
                                checked={continousLoad}
                                disabled={
                                    isDataLoading || isPartnersDataLoading
                                }
                                onChange={(e) => {
                                    setContinousLoad(e.target.checked)
                                }}
                            />
                        </Grid2>

                        <Grid2 sm={12}>
                            <Button
                                variant="contained"
                                onClick={handleLoadDataButton}
                                disabled={
                                    isDataLoading || isPartnersDataLoading
                                }
                            >
                                Učitaj
                            </Button>
                        </Grid2>
                    </Grid2>
                </Paper>
            ) : (
                <CircularProgress />
            )}

            {isPartnersDataLoading && <CircularProgress />}

            {partnersData?.payload && partnersData.pagination && (
                <Paper>
                    <Stack gap={2} m={2}>
                        {!continousLoad &&
                            partnersData.payload.length === 0 && (
                                <Typography>
                                    “Nema neslaganja u partnerima ID{' '}
                                    {(pagination.page - 1) *
                                        pagination.pageSize}{' '}
                                    -{pagination.page * pagination.pageSize}”
                                </Typography>
                            )}
                        {partnersData.payload.length > 0 && (
                            <PartneriKomercijalnoIFinansijskoTable
                                statuses={statuses}
                                partnersData={partnersData}
                                partnersRequest={partnersRequest}
                                tolerance={data.defaultTolerancija}
                            />
                        )}
                        {isPartnersDataLoading && <CircularProgress />}
                        <Stack alignItems="center">
                            <Pagination
                                disabled={
                                    isPartnersDataLoading || continousLoad
                                }
                                count={Math.ceil(
                                    partnersData.pagination.totalCount /
                                        pagination.pageSize
                                )}
                                page={pagination.page}
                                siblingCount={2}
                                boundaryCount={3}
                                onChange={(e, v) => {
                                    setPagination((prev) => ({
                                        ...prev,
                                        page: v,
                                    }))
                                }}
                            />
                        </Stack>
                    </Stack>
                </Paper>
            )}
        </Stack>
    )
}
