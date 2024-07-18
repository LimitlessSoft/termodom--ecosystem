import { CircularProgress, Grid } from '@mui/material'
import { useEffect, useState } from 'react'
import { KorisnikCenaItem } from './KorisnikCenaItem'
import { adminApi } from '@/apis/adminApi'

export const KorisnikCene = (props: any) => {
    const [productPriceGroups, setProductPriceGroups] = useState<
        any | undefined
    >(undefined)
    const [userLevels, setUserLevels] = useState<any | undefined>(undefined)

    useEffect(() => {
        adminApi.get(`/products-prices-groups`).then((response) => {
            setProductPriceGroups(response.data)
        })
    }, [])

    useEffect(() => {
        if (props.user === undefined) return

        adminApi
            .get(`/users-product-price-levels?UserId=${props.user.id}`)
            .then((response) => {
                setUserLevels(response.data)
            })
    }, [props.user])

    return productPriceGroups === undefined ||
        props.user === undefined ||
        userLevels === undefined ? (
        <CircularProgress />
    ) : (
        <Grid container justifyContent={`center`} p={4}>
            {productPriceGroups.map((pg: any, index: number) => (
                <KorisnikCenaItem
                    key={index}
                    priceGroup={pg}
                    userLevels={userLevels}
                    userId={props.user.id}
                />
            ))}
        </Grid>
    )
}
