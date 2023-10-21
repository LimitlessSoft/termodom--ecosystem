import { useEffect, useState } from 'react'
import { ApiBase, fetchApi } from '@/app/api'

export const ProizvodiProductsList = (): JSX.Element => {

    const [products, setProducts] = useState([])
    useEffect(() => {
        fetchApi(ApiBase.Main, "/products").then((response) => {
            setProducts(response.payload)
        })
        
    }, [])

    const createData = (name: string, src: string, image: string, catalogId: string, unit: string) => {
    }

    return (
        <div>
            proizvodi products list
            <div>
            {
                products.length == 0 ? 
                "Loading..." :
                <div>
                    hello
                </div>
            }
            </div>
        </div>
    )
}