import axios from "axios";
import { getCookie } from "react-use-cookie";

export const officeApi = axios.create({
    baseURL: process.env.NEXT_PUBLIC_API_BASE_MAIN_URL,
    headers: {
        Authorization: `Bearer ${getCookie('token')}`,
    }
})