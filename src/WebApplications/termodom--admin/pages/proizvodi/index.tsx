import { useEffect, useState } from "react"
import { apiGetAsync } from "../../api/api"
import constants from '../../constants'
import ProizvodRow from '../../components/porizvodi/proizvodRow'
import Link from "next/link"

export default function Proizvodi() {

    const [products, setProducts] = useState<any>(null)

    useEffect(() => {
        apiGetAsync("/products")
        .then(response => {
            if(response.status == 200) {
                response.json()
                .then((lsResponse: any) => {
                    setProducts(lsResponse.payload)
                })
            } else {
                console.error(constants.errorCommunicatingWithApiMessage)
            }
        })
        .catch(error => console.error(error))
    }, [])

    return (
        <div className={`p-2 max-w-screen-lg mx-auto`}>
            {
                !products ?
                    <div>loading</div> :
                    <Content products={products} />
             }
        </div>
    )
}

const Content = (props: any) => {
    return (
        <div>
            <div>
                <Link href="/proizvodi/novi">Dodaj novi proizvod</Link>
            </div>
            <table className={`table-auto w-full`}>
                <thead className={`bg-gray-300 font-medium`}>
                    <ProizvodRow key={`proizvod-row-header}`} item={{ id: "Id", sku: "Kat. Br.", name: "Naziv", unit: "JM" }} />
                </thead>    
                <tbody className={`[&>*:nth-child(odd)]:bg-gray-200 [&>*:nth-child(even)]:bg-gray-100 `}>
                {
                    props.products.map((product: any) => {
                        return <ProizvodRow key={`proizvod-row-${product.id}}`} item={product} />
                    })
                }
                </tbody>
            </table>
        </div>
        )
}