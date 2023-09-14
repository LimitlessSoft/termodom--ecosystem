import { useRouter } from 'next/router'
import styles from './ProizvodiProductsListItem.module.css'

export const ProizvodiProductsListItem = ({ product }: any): JSX.Element => {

    const router = useRouter()

    return (
        <tr className={`odd:bg-gray-100 even:bg-gray-0 hover:bg-gray-200 hover:cursor-pointer`}
        onClick={() => {
            router.push("/hello")
        }}>
            <td className={`p-2 border-b `}>{ product.catalogId }</td>
            <td className={`p-2 border-b`}>{ product.name }</td>
        </tr>
    )
}