import { ApiBase, fetchApi } from '@/app/api'
import { useUser } from '@/app/hooks'
import { ProizvodCard } from '@/widgets/Proizvodi/ProizvodiList/ui/ProizvodCard'
import { Grid, LinearProgress } from '@mui/material'
import { useEffect, useState } from 'react'

export const SuggestedProducts = (props: any) => {
    const user = useUser(false, false)
    const [suggestedProducts, setSuggestedProducts] = useState<
        any[] | undefined
    >([])

    useEffect(() => {
        fetchApi(
            ApiBase.Main,
            `/suggested-products?BaseProductId=${props.baseProductId}`,
            undefined
        ).then((response) => {
            response.json().then((response: any) => {
                setSuggestedProducts(response.payload)
            })
        })
    }, [props.baseProductId])

    return (
        <Grid container justifyContent={'center'}>
            {suggestedProducts === undefined && <LinearProgress />}
            {suggestedProducts !== undefined &&
                suggestedProducts.map((product) => {
                    return (
                        <ProizvodCard
                            key={product.id}
                            proizvod={product}
                            user={user}
                        />
                    )
                })}
        </Grid>
    )
}
