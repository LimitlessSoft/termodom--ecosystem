export default function ProizvodRow(props: any) {

    const proizvod = props.item

    return (
        <tr className={``}>
            <td className={`px-4`}>{proizvod.id}</td>
            <td className={`px-4`}>{proizvod.sku}</td>
            <td className={`px-4`}>{proizvod.name}</td>
            <td className={`px-4`}>{proizvod.unit}</td>
        </tr>
    )
}