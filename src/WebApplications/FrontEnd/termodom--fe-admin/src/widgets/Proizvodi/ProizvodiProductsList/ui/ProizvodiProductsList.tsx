import { useEffect, useState } from 'react'
import styles from './ProizvodiProductsList.module.css'
import { ApiBase, fetchApi } from '@/app/api'
import { ProizvodiProductsListItem } from './ProizvodiProductsListItem'

export const ProizvodiProductsList = (): JSX.Element => {

    const [products, setProducts] = useState([])
    useEffect(() => {
        fetchApi(ApiBase.Main, "/products").then((response) => {
            setProducts(response.payload)
        })
        
    }, [])
    return (
        <div>
            proizvodi products list
            <div>
            {
                products.length == 0 ? 
                "Loading..." :
                <table className={`w-full table-auto border border-collapse text-left`}>
                    <thead>
                        <tr>
                            <th className={`font-bold p-2 border-b`}>Kataloški Broj</th>
                            <th className={`font-bold p-2 border-b`}>Naziv</th>
                        </tr>
                    </thead>
                    <tbody>
                        {
                        products.map((product: any, index) => {
                            return <ProizvodiProductsListItem
                                key={`${product.src}${product.key}${index}`}
                                product={product} />
                        })
                        }
                    </tbody>
                </table>
            }
            </div>
        </div>
    )
}