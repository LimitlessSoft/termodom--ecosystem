import { PorudzbinaActionBar } from '@/widgets/Porudzbine/PorudzbinaActionbar'
import { PorudzbinaAdminInfo } from '@/widgets/Porudzbine/PorudzbinaAdminInfo'
import { PorudzbinaSummary } from '@/widgets/Porudzbine/PorudzbinaSummary'
import { PorudzbinaHeader } from '@/widgets/Porudzbine/PorudzbinaHeader'
import { PorudzbinaItems } from '@/widgets/Porudzbine/PorudzbinaItems'
import { IPorudzbina } from '@/widgets/Porudzbine/models/IPorudzbina'
import { CircularProgress, Grid } from '@mui/material'
import { UIDimensions } from '@/constants'
import { useEffect, useState } from 'react'
import { useRouter } from 'next/router'
import { LSBackButton } from 'ls-core-next'
import { adminApi, handleApiError } from '@/apis/adminApi'

const Porudzbina = (): JSX.Element => {
    const router = useRouter()
    const oneTimeHash = router.query.hash

    const [isPretvorUpdating, setIsPretvorUpdating] = useState<boolean>(false)
    const [isDisabled, setIsDisabled] = useState<boolean>(false)

    const [porudzbina, setPorudzbina] = useState<IPorudzbina | undefined>(
        undefined
    )

    const reloadPorudzbina = (callback?: () => void) => {
        adminApi
            .get(`/orders/${oneTimeHash}`)
            .then((response) => {
                setPorudzbina(response.data)
            })
            .catch((err) => handleApiError(err))
            .finally(() => {
                if (callback) callback()
            })
    }

    useEffect(() => {
        if (!oneTimeHash) {
            return setPorudzbina(undefined)
        }

        reloadPorudzbina()
    }, [oneTimeHash])

    return !porudzbina ? (
        <CircularProgress />
    ) : (
        <Grid
            sx={{
                maxWidth: UIDimensions.maxWidth,
                margin: `auto`,
            }}
        >
            <LSBackButton
                href={`/korisnici/${porudzbina.username}/porudzbine?userId=${porudzbina.userInformation.id}`}
            />
            <PorudzbinaHeader
                isDisabled={isDisabled}
                porudzbina={porudzbina}
                isTDNumberUpdating={isPretvorUpdating}
                onMestoPreuzimanjaChange={(storeId: number) => {
                    setPorudzbina((prevPorudzbina): any => ({
                        ...prevPorudzbina,
                        storeId: storeId,
                    }))
                }}
            />
            <PorudzbinaActionBar
                isDisabled={isDisabled}
                porudzbina={porudzbina}
                onPreuzmiNaObraduStart={() => {
                    setIsDisabled(true)
                }}
                onPreuzmiNaObraduEnd={() => {
                    reloadPorudzbina(() => {
                        setIsDisabled(false)
                    })
                }}
                onPretvoriUProracunStart={() => {
                    setIsDisabled(true)
                    setIsPretvorUpdating(true)
                }}
                onPretvoriUPonuduStart={() => {}}
                onRazveziOdProracunaStart={() => {
                    setIsDisabled(true)
                    setIsPretvorUpdating(true)
                }}
                onPretvoriUProracunSuccess={() => {
                    reloadPorudzbina(() => {
                        setIsDisabled(false)
                        setIsPretvorUpdating(false)
                    })
                }}
                onPretvoriUProracunFail={() => {}}
                onPretvoriUPonuduEnd={() => {}}
                onRazveziOdProracunaEnd={() => {
                    reloadPorudzbina(() => {
                        setIsDisabled(false)
                        setIsPretvorUpdating(false)
                    })
                }}
                onStornirajStart={() => {
                    setIsDisabled(true)
                }}
                onStornirajSuccess={() => {
                    reloadPorudzbina(() => {
                        setIsDisabled(false)
                    })
                }}
                onStornirajFail={() => {
                    setIsDisabled(false)
                }}
            />
            <PorudzbinaAdminInfo porudzbina={porudzbina} />
            <PorudzbinaItems porudzbina={porudzbina} />
            <PorudzbinaSummary porudzbina={porudzbina} />
        </Grid>
    )
}

export default Porudzbina
