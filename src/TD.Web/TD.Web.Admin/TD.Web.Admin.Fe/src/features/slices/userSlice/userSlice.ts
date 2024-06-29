import { ApiBase, fetchApi } from "@/app/api"
import { RootState } from "@/app/store"
import { createAsyncThunk, createSlice } from "@reduxjs/toolkit"

interface UserData {
    username: string
}

export interface User {
    isLoading: boolean,
    isLogged?: boolean | null,
    data?: UserData | null
}

const initialState: User = {
    isLoading: false,
    isLogged: null,
    data: null
}

export const fetchMe = createAsyncThunk<any>('user/fetchMe', async () => await fetchApi(ApiBase.Main, "/me", {
        method: 'GET'
    }).then(async (response) => {
        let rr
        await response.json()
            .then((r: any) => {
                rr = r
            })
        return rr
    }))

export const userSlice = createSlice({
    name: 'userSlice',
    initialState,
    reducers: { },
    extraReducers: (builder) => {
        builder.addCase(fetchMe.pending, (state, action) => {
            state.isLoading = true
            state.isLogged = false
            state.data = null
        })
        builder.addCase(fetchMe.fulfilled, (state, action: any) => {
            state.isLoading = false
            state.isLogged = action.payload.isLogged
            state.data = action.payload.userData
        })
    }
})

export const { } = userSlice.actions
export const selectUser = (state: RootState) => state.user
export default userSlice.reducer