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

    const handleSaveUserData = (fieldKey, value, params, label) => {
        const editableFields = USERS_CONSTANTS.SINGLE_USER_DATA_FIELDS.EDITABLE
        const field = editableFields[fieldKey]

        if (!field) return

        const endpointKey = `UPDATE_${fieldKey}`
        const endpoint = ENDPOINTS[endpointKey]?.(data.id)

        if (!endpoint) return

        return officeApi
            .put(endpoint, {
                id: data.id,
                ...params,
            })
            .then(() => {
                setData((prevData) => ({
                    ...prevData,
                    [field.KEY]: value,
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
                />
            )}
        </Grid>
    )
}

export default KorisniciId
