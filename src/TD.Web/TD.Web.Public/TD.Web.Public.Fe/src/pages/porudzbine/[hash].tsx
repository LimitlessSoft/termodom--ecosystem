import { PorudzbinaAdminInfo } from '@/widgets/Porudzbine/PorudzbinaAdminInfo'
import { PorudzbinaSummary } from '@/widgets/Porudzbine/PorudzbinaSummary'
import { PorudzbinaHeader } from '@/widgets/Porudzbine/PorudzbinaHeader'
import { PorudzbinaItems } from '@/widgets/Porudzbine/PorudzbinaItems'
import { IPorudzbina } from '@/widgets/Porudzbine/models/IPorudzbina'
import { CircularProgress, Grid } from '@mui/material'
import { UIDimensions } from '@/app/constants'
import { useEffect, useState } from 'react'
import { useRouter } from 'next/router'
import { handleApiError, webApi } from '@/api/webApi'
import { IStockType } from '@/widgets/Porudzbine/PorudzbinaItemRow/interfaces/IStockType'
import { IStore } from '@/widgets/Porudzbine/interfaces/IStore'
import { STORE_NAMES } from '@/widgets/Porudzbine/constants'
import { CustomHead } from '@/widgets/CustomHead'

const Porudzbina = (): JSX.Element => {
    const router = useRouter()
    const oneTimeHash = router.query.hash

    const [isPretvorUpdating, setIsPretvorUpdating] = useState<boolean>(false)
    const [isDisabled, setIsDisabled] = useState<boolean>(false)

    const [porudzbina, setPorudzbina] = useState<IPorudzbina | undefined>(
        undefined
    )
    const [stockTypes, setStockTypes] = useState<IStockType[]>([])
    const [stores, setStores] = useState<IStore[]>([])

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
