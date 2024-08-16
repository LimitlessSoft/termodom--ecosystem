import { getCookie } from 'react-use-cookie'
import { COOKIES } from '@/constants'
import axios from 'axios'
import { toast } from 'react-toastify'

export const adminApi = axios.create({
    baseURL: process.env.NEXT_PUBLIC_API_BASE_MAIN_URL,
    headers: {
        Authorization: `Bearer ${getCookie(COOKIES.TOKEN.NAME)}`,
    },
})

export const handleApiError = (error: any) => {
    switch (error.response.status) {
        case 400:
            if (error.response.data) {
                toast.error(error.response.data)
            } else {
                toast.error('Greška 400')
            }
            break
        case 401:
            toast.error('Niste autentikovani')
            break
        case 403:
            toast.error('Nemate pravo pristupa')
            break
        case 404:
            toast.error('Nije pronađeno')
            break
        case 500:
            toast.error('Greška na serveru')
            break
        default:
            toast.error('Greška')
            break
    }
}
