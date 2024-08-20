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

export const handleApiError = (error: any) => {
    switch (error.response.status) {
        case 400:
            if (!error.response.data) {
                return toast.error('Greška 400')
            }

            if (Array.isArray(error.response.data)) {
                const errorMessages = error.response.data
                    .map((item: any) => item.ErrorMessage)
                    .filter((msg: string | null) => msg)

                errorMessages.forEach((message: string) => {
                    toast.error(message)
                })
                return
            }

            return toast.error(error.response.data)
        case 401:
            return toast.error('Niste autentikovani')
        case 403:
            return toast.error('Nemate pravo pristupa')
        case 404:
            return toast.error('Nije pronađeno')
        case 500:
            return toast.error('Greška na serveru')
        default:
            return toast.error('Greška')
    }
}
