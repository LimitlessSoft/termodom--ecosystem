import { ProizvodiProductsFilter } from './ProizvodiProductsFilter'
import { StripedDataGrid } from '@/widgets/StripedDataGrid'
import { GridActionsCellItem } from '@mui/x-data-grid'
import { Badge, LinearProgress, Tooltip, Typography } from '@mui/material'
import { useEffect, useState } from 'react'
import { Edit, Info } from '@mui/icons-material'
import { useRouter } from 'next/router'
import { adminApi, handleApiError } from '@/apis/adminApi'
import { Popup } from '@mui/base/Unstable_Popup/Popup'
import { formatNumber } from '@/helpers/numberHelpers'
import { proizvodiProductsListConstants } from '@/widgets/Proizvodi/ProizvodiProductsList/proizvodiProductsListConstants'

export const ProizvodiProductsList = (): JSX.Element => {
    const [searchFilter, setSearchFilter] = useState<string>('')
    const [statusesFilter, setStatusesFiler] = useState<number[]>([])
    const [products, setProducts] = useState<any | undefined>(null)
    const [isFetching, setIsFetching] = useState<boolean>(false)

    useEffect(() => {
        setIsFetching(true)

        let url = `/products?`
        if (searchFilter != null && searchFilter.length > 0)
            url += `searchFilter=${searchFilter}`
        if (statusesFilter != null && statusesFilter.length > 0)
            url += `&status=${statusesFilter.join('&status=')}`
        adminApi
            .get(url)
            .then((response) => {
                setProducts(response.data)
                setIsFetching(false)
            })
            .catch((err) => handleApiError(err))
    }, [searchFilter, statusesFilter])

    return (
        <div>
            {products == null ? (
                <LinearProgress />
            ) : (
                <div style={{ width: '100%' }}>
                    <ProizvodiProductsFilter
                        isFetching={isFetching}
                        currentProducts={products}
                        onPretrazi={(e: string, statuses: number[]) => {
                            setSearchFilter(e)
                            setStatusesFiler(statuses)
                        }}
                    />
                    <StripedDataGrid
                        autoHeight
                        sx={{ m: 2 }}
                        rows={products}
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
                </div>
            )}
        </div>
    )
}
