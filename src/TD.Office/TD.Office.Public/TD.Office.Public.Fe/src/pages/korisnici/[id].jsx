import {
    HorizontalActionBar,
    HorizontalActionBarButton,
    KorisniciNovaLozinka,
    KorisniciSingular,
} from '@/widgets'
import { CircularProgress, Grid, Typography } from '@mui/material'
import { useRouter } from 'next/router'
import { useEffect, useState } from 'react'
import { handleApiError, officeApi } from '@/apis/officeApi'
import { ENDPOINTS_CONSTANTS, USERS_CONSTANTS } from '@/constants'
import { toast } from 'react-toastify'

const KorisniciId = () => {
    const router = useRouter()

    const [id, setId] = useState(undefined)
    const [data, setData] = useState(undefined)

    const [novaLozinkaIsOpen, setNovaLozinkaIsOpen] = useState(false)

    const FIELD_KEYS = USERS_CONSTANTS.SINGLE_USER_DATA_FIELD_KEYS
    const FIELD_LABELS = USERS_CONSTANTS.SINGLE_USER_DATA_FIELD_LABELS
    const ENDPOINTS = ENDPOINTS_CONSTANTS.USERS

    useEffect(() => {
        if (router.query.id) setId(router.query.id)
        else setId(undefined)
    }, [router, router.query.id])

    useEffect(() => {
        setData(undefined)

        if (!id) return

        officeApi
            .get(`/users/${id}`)
            .then((response) => {
                setData(response.data)
            })
            .catch((err) => handleApiError(err))
    }, [id])

    const handleSaveUserData = (fieldName, value, params, label) => {
        const fieldsApiEndpointMap = {
            [FIELD_KEYS.NADIMAK]: ENDPOINTS.UPDATE_NICKNAME(data.id),
            [FIELD_KEYS.MAX_RABAT_MP_DOKUMENTI]:
                ENDPOINTS.UPDATE_MAX_RABAT_MP_DOKUMENTI(data.id),
            [FIELD_KEYS.MAX_RABAT_VP_DOKUMENTI]:
                ENDPOINTS.UPDATE_MAX_RABAT_VP_DOKUMENTI(data.id),
        }

        return officeApi
            .put(fieldsApiEndpointMap[fieldName], {
                id: data.id,
                ...params,
            })
            .then(() => {
                setData((prevData) => ({
                    ...prevData,
                    [fieldName]: value,
                }))
                toast.success(
                    `Uspešno promenjeno polje "${label.toLowerCase()}".`
                )
            })
            .catch(handleApiError)
    }

    return (
        <Grid>
            <KorisniciNovaLozinka
                id={id}
                isOpen={novaLozinkaIsOpen}
                onClose={() => {
                    setNovaLozinkaIsOpen(false)
                }}
            />
            <HorizontalActionBar>
                <HorizontalActionBarButton
                    text="Nazad"
                    onClick={() => router.push(`/korisnici`)}
                />
                <HorizontalActionBarButton
                    color={`secondary`}
                    text="Postavi novu lozinku"
                    onClick={() => setNovaLozinkaIsOpen(true)}
                />
            </HorizontalActionBar>
            {data === undefined ? (
                <CircularProgress />
            ) : data === null ? (
                <Typography>Korisnik nije pronađen</Typography>
            ) : (
                <KorisniciSingular
                    user={data}
                    onSaveUserData={handleSaveUserData}
                    FIELD_KEYS={FIELD_KEYS}
                    FIELD_LABELS={FIELD_LABELS}
                />
            )}
        </Grid>
    )
}

export default KorisniciId
