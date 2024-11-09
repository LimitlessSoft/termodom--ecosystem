import {
    Button,
    Paper,
    Stack,
    TextField,
    CircularProgress,
    Pagination,
    Box,
} from '@mui/material'
import React, { useEffect, useState } from 'react'
import { handleApiError, officeApi } from '@/apis/officeApi'
import Grid2 from '@mui/material/Unstable_Grid2'
import qs from 'qs'
import {
    ENDPOINTS_CONSTANTS,
    PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS,
} from '@/constants'
import PartneriKomercijalnoIFinansijskoTable from '@/widgets/Partneri/PartneriFinansijskoIKomercijalno/helpers/ui/PartneriKomercijalnoIFinansijskoTable'
import { ComboBoxInput } from '@/widgets'

export default function PartneriKomercijalnoIFinansijsko() {
    const [data, setData] = useState(undefined)

    const [partnersRequest, setPartnersRequest] = useState({
        search: '',
        years: [],
        tolerancija: 0,
    })

    const [partnersData, setPartnersData] = useState(undefined)
    const [pagination, setPagination] = useState({
        pageSize:
            PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS.INITIAL_PAGE_SIZE,
        page: PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS.INITIAL_PAGE,
    })

    useEffect(() => {
        getPartnersData()
    }, [pagination])

    useEffect(() => {
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
    }, [])

    const getPartnersData = () => {
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
            .then((data) => setPartnersData(data))
            .catch(handleApiError)
    }

    const handleLoadDataButton = (e) => {
        e.preventDefault()

        if (partnersRequest.years.length === 0) return

        getPartnersData()
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
                            />
                        </Grid2>

                        <Grid2>
                            <TextField
                                label="Pretraga"
                                variant="outlined"
                                onChange={handleSearchChange}
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

            {partnersData?.payload && partnersData?.pagination && (
                <Paper>
                    <Stack gap={2} m={2}>
                        <PartneriKomercijalnoIFinansijskoTable
                            partnersData={partnersData}
                            partnersRequest={partnersRequest}
                            tolerance={data.defaultTolerancija}
                        />
                        <Stack alignItems={`center`}>
                            <Pagination
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
