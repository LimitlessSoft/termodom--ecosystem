import { useEffect, useState } from 'react'
import { ApiBase, fetchApi } from '@/app/api'
import { Paper, Table, TableCell, TableContainer, TableHead } from '@mui/material'

export const ProizvodiProductsList = (): JSX.Element => {

    const [products, setProducts] = useState([])
    useEffect(() => {
        fetchApi(ApiBase.Main, "/products").then((response) => {
            setProducts(response.payload)
        })
        
    }, [])

    const createData = (name: string, src: string, image: string,
        catalogId: string, unit: string, classification: string,
        vat: number, productPriceGroup: string) => {
            return { name, src, image, catalogId, unit, classification, vat, productPriceGroup }
    }

    return (
        <div>
            proizvodi products list
            <div>
            {
                products.length == 0 ? 
                "Loading..." :
                <TableContainer component={Paper}>
                    <Table sx={{ minWidth: 650 }} aria-label="Simple table">
                        <TableHead>
                            <TableCell>Kataloški broj</TableCell>
                            <TableCell>Naziv</TableCell>
                        </TableHead>
                    </Table>
                </TableContainer>
            }
            </div>
        </div>
    )
}