import { getCookie } from 'react-use-cookie'
import { COOKIES } from '@/constants'
import axios from 'axios'

const _webApi = () =>
    axios.create({
        baseURL: process.env.NEXT_PUBLIC_API_BASE_MAIN_URL,
        headers: {
            Authorization: `Bearer ${getCookie(COOKIES.TOKEN.NAME)}`,
        },
    })

export const webApi = _webApi()
