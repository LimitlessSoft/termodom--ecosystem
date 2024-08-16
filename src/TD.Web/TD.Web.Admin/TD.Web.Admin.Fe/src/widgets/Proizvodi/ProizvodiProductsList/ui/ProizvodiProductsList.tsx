import { ProizvodiProductsFilter } from './ProizvodiProductsFilter'
import { StripedDataGrid } from '@/widgets/StripedDataGrid'
import { GridActionsCellItem } from '@mui/x-data-grid'
import { LinearProgress } from '@mui/material'
import { useEffect, useState } from 'react'
import { Edit } from '@mui/icons-material'
import { useRouter } from 'next/router'
import { adminApi, handleApiError } from '@/apis/adminApi'

export const ProizvodiProductsList = (): JSX.Element => {
    const router = useRouter()
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
