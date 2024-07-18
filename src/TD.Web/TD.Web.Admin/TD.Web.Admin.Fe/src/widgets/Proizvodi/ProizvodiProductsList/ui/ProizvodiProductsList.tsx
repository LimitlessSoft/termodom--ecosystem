import { ProizvodiProductsFilter } from './ProizvodiProductsFilter'
import { StripedDataGrid } from '@/widgets/StripedDataGrid'
import { GridActionsCellItem } from '@mui/x-data-grid'
import { LinearProgress } from '@mui/material'
import { ApiBase, fetchApi } from '@/api'
import { useEffect, useState } from 'react'
import { Edit } from '@mui/icons-material'
import { useRouter } from 'next/router'

export const ProizvodiProductsList = (): JSX.Element => {
    const router = useRouter()
    const [searchFilter, setSearchFilter] = useState<string>('')
    const [products, setProducts] = useState<any | undefined>(null)
    const [isFetching, setIsFetching] = useState<boolean>(false)

    useEffect(() => {
        setIsFetching(true)
        fetchApi(ApiBase.Main, `/products?searchFilter=${searchFilter}`).then(
            (payload) => {
                payload.json().then((data: any) => {
                    setProducts(data)
                    setIsFetching(false)
                })
            }
        )
    }, [searchFilter])

    return (
        <div>
            {products == null ? (
                <LinearProgress />
            ) : (
                <div style={{ width: '100%' }}>
                    <ProizvodiProductsFilter
                        isFetching={isFetching}
                        onPretrazi={(e: string) => {
                            setSearchFilter(e)
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
                                            onClick={() => {
                                                router.push(
                                                    `/proizvodi/izmeni/${params.id}`
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
