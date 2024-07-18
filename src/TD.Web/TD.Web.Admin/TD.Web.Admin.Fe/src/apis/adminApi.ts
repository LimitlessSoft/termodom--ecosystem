import { getCookie } from 'react-use-cookie'
import axios from 'axios'

export const adminApi = axios.create({
    baseURL: process.env.NEXT_PUBLIC_API_BASE_MAIN_URL,
    headers: {
        Authorization: `Bearer ${getCookie('token')}`,
    },
})
