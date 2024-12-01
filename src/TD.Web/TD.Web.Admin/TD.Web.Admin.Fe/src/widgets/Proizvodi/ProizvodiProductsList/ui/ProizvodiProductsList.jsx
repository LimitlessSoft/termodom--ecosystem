import { ProizvodiProductsFilter } from './ProizvodiProductsFilter'
import { StripedDataGrid } from '@/widgets/StripedDataGrid'
import { GridActionsCellItem } from '@mui/x-data-grid'
import {
    Badge,
    CircularProgress,
    LinearProgress,
    Tooltip,
    Typography,
    Stack,
} from '@mui/material'
import { useEffect, useState } from 'react'
import { Edit, Info } from '@mui/icons-material'
import { adminApi, handleApiError } from '@/apis/adminApi'
import { formatNumber } from '@/helpers/numberHelpers'
import { proizvodiProductsListConstants } from '@/widgets/Proizvodi/ProizvodiProductsList/proizvodiProductsListConstants'
import qs from 'qs'
import Grid2 from '@mui/material/Unstable_Grid2'

export const ProizvodiProductsList = () => {
    const [filters, setFilters] = useState({
        searchFilter: '',
        statusesFilter: [],
    })
    const [products, setProducts] = useState(null)
    const [isFetching, setIsFetching] = useState(false)

    useEffect(() => {
        if (!filters.searchFilter.trim() && filters.statusesFilter.length == 0)
            return

        setIsFetching(true)

        const params = {
            ...(filters.searchFilter.trim() && {
                searchFilter: filters.searchFilter,
            }),
            ...(filters.statusesFilter.length > 0 && {
                status: filters.statusesFilter,
            }),
        }

        adminApi
            .get('/products', {
                params,
                paramsSerializer: (params) =>
                    qs.stringify(params, { arrayFormat: 'repeat' }),
            })
            .then((response) => setProducts(response.data || []))
            .catch((err) => handleApiError(err))
            .finally(() => setIsFetching(false))
    }, [filters])

    return (
        <Grid2 container direction={`column`} p={2} gap={2}>
            <Grid2>
                <ProizvodiProductsFilter
                    isFetching={isFetching}
                    currentProducts={products}
                    onPretrazi={(search, statuses) =>
                        setFilters({
                            searchFilter: search,
                            statusesFilter: statuses,
                        })
                    }
                />
            </Grid2>
            <Grid2>
                {isFetching ? (
                    <CircularProgress />
                ) : (
                    <StripedDataGrid
                        autoHeight
                        rows={products || []}
                        noRowsMessage={`Nema dostupnih proizvoda za izabrani filter`}
                        columns={[
                            {
                                field: 'catalogId',
                                headerName: 'Kataloški Broj',
                                minWidth: 150,
                            },
                            {
                                field: 'name',
                                headerName: 'Naziv',
                                minWidth: 150,
                                flex: 1,
                            },
                            {
                                field: 'src',
                                headerName: 'src',
                                minWidth: 150,
                                flex: 1,
                            },
                            {
                                field: 'ironPriceWithoutVAT',
                                headerName: 'Iron Cena',
                                renderCell: (params) => {
                                    const vat = params.row.vat
                                    return (
                                        <Badge
                                            badgeContent={
                                                <Tooltip
                                                    title={`Bez PDV-a: ${formatNumber(params.value)}`}
                                                    arrow
                                                    placement={`top-end`}
                                                >
                                                    <Info
                                                        sx={{
                                                            fontSize: `1rem`,
                                                        }}
                                                        color={`secondary`}
                                                    />
                                                </Tooltip>
                                            }
                                        >
                                            <Typography p={0.1}>
                                                {formatNumber(
                                                    params.value +
                                                        (params.value * vat) /
                                                            100
                                                )}{' '}
                                                RSD
                                            </Typography>
                                        </Badge>
                                    )
                                },
                                width: proizvodiProductsListConstants.PRICE_COLUMNS_WIDTH,
                            },
                            {
                                field: 'platinumPriceWithoutVAT',
                                headerName: 'Platinum Cena',
                                renderCell: (params) => {
                                    const vat = params.row.vat
                                    return (
                                        <Badge
                                            badgeContent={
                                                <Tooltip
                                                    title={`Bez PDV-a: ${formatNumber(params.value)}`}
                                                    arrow
                                                    placement={`top-end`}
                                                >
                                                    <Info
                                                        sx={{
                                                            fontSize: `1rem`,
                                                        }}
                                                        color={`secondary`}
                                                    />
                                                </Tooltip>
                                            }
                                        >
                                            <Typography p={0.1}>
                                                {formatNumber(
                                                    params.value +
                                                        (params.value * vat) /
                                                            100
                                                )}{' '}
                                                RSD
                                            </Typography>
                                        </Badge>
                                    )
                                },
                                width: proizvodiProductsListConstants.PRICE_COLUMNS_WIDTH,
                            },
                            {
                                field: 'actions',
                                headerName: 'Akcije',
                                type: 'actions',
                                getActions: (params) => {
                                    return [
                                        <GridActionsCellItem
                                            key={`edit-product-${params.id}`}
                                            icon={<Edit />}
                                            label="Izmeni"
                                            disabled={!params.row.canEdit}
                                            onClick={() => {
                                                window.open(
                                                    `/proizvodi/izmeni/${params.id}`,
                                                    '_blank'
                                                )
                                            }}
                                        />,
                                    ]
                                },
                            },
                        ]}
                        initialState={{
                            pagination: {
                                paginationModel: { page: 0, pageSize: 10 },
                            },
                        }}
                        getRowClassName={(params) =>
                            params.indexRelativeToCurrentPage % 2 === 0
                                ? 'even'
                                : 'odd'
                        }
                        pageSizeOptions={[5, 10]}
                    />
                )}
            </Grid2>
        </Grid2>
    )
}
