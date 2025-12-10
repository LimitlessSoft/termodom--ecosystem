import { PorudzbinaAdminInfo } from '@/widgets/Porudzbine/PorudzbinaAdminInfo'
import { PorudzbinaSummary } from '@/widgets/Porudzbine/PorudzbinaSummary'
import { PorudzbinaHeader } from '@/widgets/Porudzbine/PorudzbinaHeader'
import { PorudzbinaItems } from '@/widgets/Porudzbine/PorudzbinaItems'
import { CircularProgress, Grid } from '@mui/material'
import { UIDimensions } from '@/app/constants'
import { useEffect, useState } from 'react'
import { useRouter } from 'next/router'
import { handleApiError, webApi } from '@/api/webApi'
import { STORE_NAMES } from '@/widgets/Porudzbine/constants'
import { CustomHead } from '@/widgets/CustomHead'

const Porudzbina = () => {
    const router = useRouter()
    const oneTimeHash = router.query.hash

    const [isPretvorUpdating, setIsPretvorUpdating] = useState(false)
    const [isDisabled, setIsDisabled] = useState(false)

    const [porudzbina, setPorudzbina] = useState(undefined)
    const [stockTypes, setStockTypes] = useState([])
    const [stores, setStores] = useState([])

    const reloadPorudzbina = () => {
        Promise.all([
            webApi.get(`/orders/${oneTimeHash}`),
            webApi.get(`/product-stock-types`),
            webApi.get(`/stores`),
        ])
            .then(([order, stockTypes, stores]) => {
                setPorudzbina(order.data)
                setStockTypes(stockTypes.data)
                setStores(stores.data)
            })
            .catch(handleApiError)
    }

    useEffect(() => {
        if (oneTimeHash == null) {
            setPorudzbina(undefined)
            return
        }

        console.log(router)

        reloadPorudzbina()
    }, [oneTimeHash])

    const isDelivery = stores.some(
        (store) =>
            store.id === porudzbina?.storeId &&
            store.name === STORE_NAMES.DOSTAVA
    )

    return !porudzbina ? (
        <CircularProgress />
    ) : (
        <Grid
            sx={(theme) => ({
                maxWidth: UIDimensions.maxWidth,
                m: `${theme.spacing(3)} auto`,
            })}
        >
            <CustomHead
                title={`Pregled Porudzbine - ${oneTimeHash} | Termodom`}
            />
            <PorudzbinaHeader
                isDisabled={isDisabled}
                porudzbina={porudzbina}
                isTDNumberUpdating={isPretvorUpdating}
            />
            <PorudzbinaAdminInfo
                porudzbina={porudzbina}
                stockTypes={stockTypes}
                isDelivery={isDelivery}
            />
            <PorudzbinaItems
                items={porudzbina.items}
                stockTypes={stockTypes}
                isDelivery={isDelivery}
            />
            <PorudzbinaSummary porudzbina={porudzbina} />
        </Grid>
    )
}

export default Porudzbina
