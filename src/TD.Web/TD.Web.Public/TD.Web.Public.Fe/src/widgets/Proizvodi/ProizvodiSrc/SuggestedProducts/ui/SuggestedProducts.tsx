import { ApiBase, fetchApi } from "@/app/api";
import { useUser } from "@/app/hooks";
import { ProizvodCard } from "@/widgets/Proizvodi/ProizvodiList/ui/ProizvodCard";
import { LinearProgress, Stack } from "@mui/material"
import { useEffect, useState } from "react"

export const SuggestedProducts = (): JSX.Element => {
    const user = useUser(false, false)
    const [suggestedProducts, setSuggestedProducts] = useState<any[] | undefined>([])

    useEffect(() => {
        fetchApi(ApiBase.Main, `/suggested-products`)
        .then((response) => {
            setSuggestedProducts(response)
        })
    }, [])

    useEffect(() => {
        console.log(user)
    }, [user])
    
    return (
        <Stack width={`100%`} direction={`row`}>
            { suggestedProducts === undefined && <LinearProgress /> }
            { suggestedProducts !== undefined && suggestedProducts.map((product) => {
                return (
                    <ProizvodCard key={product.id} proizvod={product} user={user} />
                )})
            }
        </Stack>
    )
}