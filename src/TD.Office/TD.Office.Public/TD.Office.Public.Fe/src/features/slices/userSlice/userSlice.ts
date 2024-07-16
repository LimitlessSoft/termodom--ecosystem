import {createAsyncThunk,createSlice} from "@reduxjs/toolkit"
import {officeApi} from "@/apis/officeApi"
import {RootState} from "@/store"

interface UserData {
    username: string,
    storeId?: number
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

export const fetchMe = createAsyncThunk<any>('user/fetchMe', async () => await officeApi.get(`/me`)
    .then(async (response: any) => {
        return response.data
    }))

export const userSlice = createSlice({
    name: 'userSlice',
    initialState,
    reducers: { },
    extraReducers: (builder) => {
        builder.addCase(fetchMe.pending, (state, action) => {
            state.isLoading = true
            state.isLogged = null
            state.data = null
        })
        builder.addCase(fetchMe.fulfilled, (state, action) => {
            state.isLoading = false
            state.isLogged = action.payload.isLogged
            state.data = action.payload.userData
        })
    }
})

export const { } = userSlice.actions
export const selectUser = (state: RootState) => state.user
export default userSlice.reducer