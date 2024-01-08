import { ApiBase, fetchApi } from "@/app/api"
import { createAsyncThunk } from "@reduxjs/toolkit"

interface UserData {
    username: string
}

interface User {
    isLoading: boolean,
    isLogged: boolean,
    data?: UserData | null
}

const initialState: User = {
    isLoading: false,
    isLogged: false,
    data: null
}

export const fetchMe = createAsyncThunk<any>('user/fetchMe', async () => await fetchApi(ApiBase.Main, "/me", {
    method: 'GET'
}).then((response) => {
    return response
}))