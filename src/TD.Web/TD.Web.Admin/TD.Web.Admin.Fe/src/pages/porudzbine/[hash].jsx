import { PorudzbinaActionBar } from '@/widgets/Porudzbine/PorudzbinaActionbar'
import { PorudzbinaAdminInfo } from '@/widgets/Porudzbine/PorudzbinaAdminInfo'
import { PorudzbinaSummary } from '@/widgets/Porudzbine/PorudzbinaSummary'
import { PorudzbinaHeader } from '@/widgets/Porudzbine/PorudzbinaHeader'
import { PorudzbinaItems } from '@/widgets/Porudzbine/PorudzbinaItems'
import KomentarInput from '@/widgets/KomentarInput'
import { CircularProgress, Grid } from '@mui/material'
import { STYLES_CONSTANTS } from '@/constants'
import { useEffect, useState } from 'react'
import { useRouter } from 'next/router'
import { LSBackButton } from 'ls-core-next'
import { adminApi, handleApiError } from '@/apis/adminApi'

const Porudzbina = () => {
    const router = useRouter()
    const oneTimeHash = router.query.hash

    const [isPretvorUpdating, setIsPretvorUpdating] = useState(false)
    const [isDisabled, setIsDisabled] = useState(false)

    const [porudzbina, setPorudzbina] = useState(undefined)

    const reloadPorudzbina = (callback) => {
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

    const handleSaveComment = (value, commentType) => {
        adminApi
            .put(`/orders/${oneTimeHash}/${commentType}-comment`, {
                oneTimeHash: oneTimeHash,
                comment: value,
            })
            .catch(handleApiError)
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
                maxWidth: STYLES_CONSTANTS.UI_DIMENSIONS.MAX_WIDTH,
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
                onMestoPreuzimanjaChange={(storeId) => {
                    setPorudzbina((prevPorudzbina) => ({
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
            <Grid container px={2} gap={2} alignItems={`center`}>
                <Grid item xs={8}>
                    <KomentarInput
                        label={`Komentar`}
                        defaultValue={porudzbina.publicComment || ''}
                        onSave={(value) => handleSaveComment(value, 'public')}
                    />
                    <KomentarInput
                        label={`Interni komentar`}
                        defaultValue={porudzbina.adminComment || ''}
                        onSave={(value) => handleSaveComment(value, 'admin')}
                    />
                </Grid>
                <PorudzbinaSummary porudzbina={porudzbina} />
            </Grid>
        </Grid>
    )
}

export default Porudzbina
