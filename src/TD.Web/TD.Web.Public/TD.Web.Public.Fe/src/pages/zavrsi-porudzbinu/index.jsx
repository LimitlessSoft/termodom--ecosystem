import { webApi, handleApiError } from '@/api/webApi'
import {
    CookieNames,
    UIDimensions,
    OrderConclusionTitle,
} from '@/app/constants'
import { OrderConclusion } from '@/widgets/Order/ui'
import { CircularProgress, Stack, Typography } from '@mui/material'
import { useRouter } from 'next/router'
import { useEffect, useState } from 'react'
import { CustomHead } from '@/widgets/CustomHead'
import {
    HorizontalActionBar,
    HorizontalActionBarButton,
} from '@/widgets/TopActionBar'
import useCookie from 'react-use-cookie'

const ZavrsiPorudzbinu = () => {
    const [cartId] = useCookie(CookieNames.cartId)
    const [orderInfo, setOrderInfo] = useState(null)
    const router = useRouter()

    useEffect(() => {
        webApi
            .get(`/checkout?oneTimeHash=${cartId}`)
            .then((res) => setOrderInfo(res.data))
            .catch((err) => {
                if (err.response.status == 404) router.push('/')
                handleApiError(err)
            })
    }, [cartId])

    return (
        <Stack
            maxWidth={UIDimensions.maxWidth}
            sx={{
                m: '24px auto',
                px: 2,
                gap: 2,
            }}
        >
            <CustomHead title={OrderConclusionTitle} />

            <HorizontalActionBar>
                <HorizontalActionBarButton
                    text={`Nazad u korpu`}
                    onClick={() => {
                        router.push(`/korpa`)
                    }}
                />
            </HorizontalActionBar>

            <Typography
                component={`h1`}
                variant={`h4`}
                fontWeight={`bold`}
                textAlign={`center`}
                my={2}
            >
                Zaključi porudžbinu
            </Typography>
            {orderInfo ? (
                <OrderConclusion
                    favoriteStoreId={orderInfo.favoriteStoreId}
                    paymentTypeId={orderInfo.paymentTypeId}
                    oneTimeHash={cartId}
                    onSuccess={() => {
                        router.push(`/porudzbine/${cartId}`)
                    }}
                />
            ) : (
                <CircularProgress />
            )}
        </Stack>
    )
}

export default ZavrsiPorudzbinu
