import { useEffect, useState } from 'react'
import { ApiBase, fetchApi } from '@/app/api'
import { LinearProgress } from '@mui/material'
import { StripedDataGrid } from '@/widgets/StripedDataGrid';
import { GridActionsCellItem } from '@mui/x-data-grid';
import { Edit } from '@mui/icons-material';
import { useRouter } from 'next/router';

export const ProizvodiProductsList = (): JSX.Element => {

    const router = useRouter()
    const [products, setProducts] = useState<any | undefined>(null)

    useEffect(() => {
        fetchApi(ApiBase.Main, "/products").then((payload) => {
            setProducts(payload)
        })
    }, [])

    return (
        <div>
        {
            products == null ? 
            <LinearProgress /> :
            <div style={{ width: '100%' }}>
                <StripedDataGrid
                    autoHeight
                    sx={{ m: 2 }}
                    rows={products}
                    columns={[
                        { field: 'catalogId', headerName: 'Kataloški Broj', minWidth: 150 },
                        { field: 'name', headerName: 'Naziv', minWidth: 150, flex: 1 },
                        { field: 'src', headerName: 'src', minWidth: 150, flex: 1 },
                        {
                            field: 'actions',
                            headerName: 'Akcije',
                            type: 'actions',
                            getActions: (params) => {
                                return [
                                    <GridActionsCellItem
                                        key={`edit-product-${params.id}`}
                                        icon={<Edit />}
                                        label='Izmeni'
                                        onClick={() => {
                                            router.push(`/proizvodi/izmeni/${params.id}`)
                                        }}
                                    />
                                ]
                            }
                        }
                    ]}
                    initialState={{
                        pagination: {
                            paginationModel: { page: 0, pageSize: 10 }
                        }
                    }}
                    getRowClassName={(params) => params.indexRelativeToCurrentPage % 2 === 0 ? 'even' : 'odd' }
                    pageSizeOptions={[5, 10]} />
            </div>
        }
        </div>
    )
}