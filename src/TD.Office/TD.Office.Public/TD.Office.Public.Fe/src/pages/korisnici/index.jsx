import { useEffect } from 'react'
import { useRouter } from 'next/router'
import { URL_CONSTANTS } from '@/constants'

const KorisniciIndex = () => {
    const router = useRouter()

    useEffect(() => {
        router.replace(URL_CONSTANTS.KORISNICI.LISTA)
    }, [router])

    return null
}

export default KorisniciIndex
