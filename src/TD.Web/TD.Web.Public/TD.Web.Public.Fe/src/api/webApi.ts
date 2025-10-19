import { getCookie } from 'react-use-cookie'
import { COOKIES } from '@/constants'
import axios from 'axios'
import { toast } from 'react-toastify'

export const webApi = axios.create({
    baseURL: process.env.NEXT_PUBLIC_API_BASE_MAIN_URL,
    headers: {
        Authorization: `Bearer ${getCookie(COOKIES.TOKEN.NAME)}`,
    },
})

export const getServerSideWebApi = (context: any) =>
    axios.create({
        baseURL: process.env.NEXT_PUBLIC_API_BASE_MAIN_URL,
        headers: {
            Authorization: `Bearer ${context.req.cookies[COOKIES.TOKEN.NAME]}`,
        },
    })

export const handleApiError = (error: any) => {
    console.log("Parsing error", error)
    if (error.code === 'ERR_CANCELED') return
    switch (error.response.status) {
        case 400:
            if (!error.response.data) {
                toast.error('Greška 400')
                return
            }

            if (Array.isArray(error.response.data)) {
                error.response.data.forEach(
                    (item: any) =>
                        item.ErrorMessage && toast.error(item.ErrorMessage)
                )
                return
            }

            toast.error(error.response.data)
            return
        case 401:
            toast.error('Niste autentikovani')
            return
        case 403:
            toast.error('Nemate pravo pristupa')
            return
        case 404:
            toast.error('Nije pronađeno')
            return
        case 500:
            toast.error('Greška na serveru')
            return
        default:
            toast.error('Greška')
            return
    }
}
