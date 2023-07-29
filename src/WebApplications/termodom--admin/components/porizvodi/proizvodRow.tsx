import Link from "next/link"
import { useRouter } from "next/router"

export default function ProizvodRow(props: any) {

    const router = useRouter()
    const proizvod = props.item

    return (
        <tr onClick={() => {
            router.push(`/proizvodi/izmeni/${proizvod.id}`)
        }} className={`hover:cursor-pointer hover:text-blue-500`}>
            <td className={`px-4`}>{proizvod.id}</td>
            <td className={`px-4`}>{proizvod.sku}</td>
            <td className={`px-4`}>{proizvod.name}</td>
            <td className={`px-4`}>{proizvod.unit}</td>
        </tr>
    )
}