import {
    Button,
    Paper,
    Stack,
    TextField,
    CircularProgress,
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
    })

    const [partnersData, setPartnersData] = useState(undefined)
    const [pagination, setPagination] = useState({
        pageSize:
            PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS.INITIAL_PAGE_SIZE,
        page: PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS.INITIAL_PAGE,
    })

    useEffect(() => {
        officeApi
            .get(ENDPOINTS_CONSTANTS.PARTNERS.GET_KOMERCIJALNO_I_FINANSIJSKO)
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
                        kraj: 160000,
                    },
                    {
                        year: 2024,
                        pocetak: 160000,
                        kraj: 180000,
                    },
                ],
                finansijskoKupac: [
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
                finansijskoDobavljac: [
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
                    pageSize: pagination.pageSize,
                },
                paramsSerializer: (params) =>
                    qs.stringify(params, { arrayFormat: 'repeat' }),
            })
            .then((res) => res.data)
            .then((data) => setPartnersData(data))
            .catch(handleApiError)
    }

    const onPaginationChange = (value) => {
        setPagination(value)
    }

    const onSelectionChange = (e) => {
        if (partnersData) setPartnersData(undefined)

        const uniqueYears = [...new Set(e.target.value.flat())]

        setPartnersRequest((prev) => ({
            ...prev,
            years: uniqueYears,
        }))
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
                        <PartneriKomercijalnoIFinansijskoTable
                            partnersData={partnersData}
                            partnersRequest={partnersRequest}
                            pagination={pagination}
                            onPaginationChange={onPaginationChange}
                            tolerance={data.defaultTolerancija}
                        />
                    </Stack>
                </Paper>
            )}
        </Stack>
    )
}
