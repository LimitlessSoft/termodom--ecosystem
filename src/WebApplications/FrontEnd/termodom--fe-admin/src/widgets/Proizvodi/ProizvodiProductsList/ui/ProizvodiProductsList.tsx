import { useEffect, useState } from 'react'
import { ApiBase, fetchApi } from '@/app/api'
import { LinearProgress } from '@mui/material'
import { StripedDataGrid } from '@/widgets/StripedDataGrid';

export const ProizvodiProductsList = (): JSX.Element => {

    const [products, setProducts] = useState([])

    useEffect(() => {
        fetchApi(ApiBase.Main, "/products").then((response) => {
            setProducts(response.payload)
        })
    }, [])

    return (
        <div>
        {
            products.length == 0 ? 
            <LinearProgress /> :
            <div style={{ width: '100%' }}>
                <StripedDataGrid
                    autoHeight
                    sx={{ m: 2 }}
                    rows={products}
                    columns={[
                        { field: 'catalogId', headerName: 'Kataloški Broj', minWidth: 150, flex: 1 },
                        { field: 'name', headerName: 'Naziv', minWidth: 150, flex: 1 },
                        { field: 'src', headerName: 'src', minWidth: 150, flex: 1}
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