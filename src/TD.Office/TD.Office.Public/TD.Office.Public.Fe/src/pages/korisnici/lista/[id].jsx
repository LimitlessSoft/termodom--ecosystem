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
import { ENDPOINTS_CONSTANTS, URL_CONSTANTS } from '@/constants'
import { toast } from 'react-toastify'
import korisniciSingularFieldsConfig from '@/data/korisniciSingularFieldsConfig.json'

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

    const handleUpdateTipKorisnika = (tipKorisnikaId, naziv, boja) => {
        setData((prevData) => ({
            ...prevData,
            tipKorisnikaId,
            tipKorisnikaNaziv: naziv,
            tipKorisnikaBoja: boja,
        }))
    }

    const handleSaveUserData = (fieldKey, value, params, label) => {
        const field = korisniciSingularFieldsConfig.FIELDS[fieldKey]

        if (!field || !field.EDITABLE) return

        if (field.VALIDATION === 'integer' && !Number.isInteger(+value)) {
            toast.error(`Polje "${label}" mora biti ceo broj.`)
            return
        }

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
                    onClick={() => router.push(URL_CONSTANTS.KORISNICI.LISTA)}
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
                    onUpdateTipKorisnika={handleUpdateTipKorisnika}
                />
            )}
        </Grid>
    )
}

export default KorisniciId
