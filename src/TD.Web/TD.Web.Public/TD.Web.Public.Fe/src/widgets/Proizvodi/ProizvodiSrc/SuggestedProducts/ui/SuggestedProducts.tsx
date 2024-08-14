import { handleApiError, webApi } from '@/api/webApi'
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
        webApi
            .get(`/suggested-products?BaseProductId=${props.baseProductId}`)
            .then((res) => {
                setSuggestedProducts(res.data.payload)
            })
            .catch((err) => handleApiError(err))
    }, [props.baseProductId])

    return (
        <Grid container justifyContent={'center'}>
            {!suggestedProducts ? (
                <LinearProgress />
            ) : (
                suggestedProducts.map((product) => {
                    return (
                        <ProizvodCard
                            key={product.id}
                            proizvod={product}
                            user={user}
                        />
                    )
                })
            )}
        </Grid>
    )
}
