import { ApiBase, fetchApi } from '@/api'
import {
    CircularProgress,
    Grid,
    MenuItem,
    TextField,
    Typography,
} from '@mui/material'
import { useEffect, useState } from 'react'
import { KorisnikCeneItemWrapperStyled } from './KorisnikCeneItemWrapperStyled'
import { KorisnikCenaItem } from './KorisnikCenaItem'

export const KorisnikCene = (props: any): JSX.Element => {
    const [productPriceGroups, setProductPriceGroups] = useState<
        any | undefined
    >(undefined)
    const [userLevels, setUserLevels] = useState<any | undefined>(undefined)

    useEffect(() => {
        fetchApi(ApiBase.Main, `/products-prices-groups`)
            .then((response) => response.json())
            .then((data) => setProductPriceGroups(data))
    }, [])

    useEffect(() => {
        if (props.user === undefined) return

        fetchApi(
            ApiBase.Main,
            `/users-product-price-levels?UserId=${props.user.id}`
        )
            .then((response) => response.json())
            .then((data) => setUserLevels(data))
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
