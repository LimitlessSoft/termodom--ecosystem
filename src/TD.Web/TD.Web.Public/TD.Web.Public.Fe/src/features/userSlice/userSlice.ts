import { handleApiError, webApi } from '@/api/webApi'
import { RootState } from '@/app/store'
import { createAsyncThunk, createSlice } from '@reduxjs/toolkit'

interface UserData {
    nickname: string
    isAdmin: boolean
}

export interface User {
    isLoading: boolean
    isLogged: boolean
    data?: UserData | null
}

const initialState: User = {
    isLoading: true,
    isLogged: false,
}

webApi
export const fetchMe = createAsyncThunk<any>(
    'user/fetchMe',
    async () =>
        await webApi
            .get('/me')
            .then((response) => response.data)
            .catch((err) => handleApiError(err))
)

export const userSlice = createSlice({
    name: 'userSlice',
    initialState,
    reducers: {},
    extraReducers: (builder) => {
        builder.addCase(fetchMe.pending, (state, action) => {
            state.isLoading = true
            state.isLogged = false
            state.data = null
        })
        builder.addCase(fetchMe.fulfilled, (state, action: any) => {
            state.isLoading = false
            state.isLogged = action.payload?.isLogged
            state.data = action.payload?.userData
        })
    },
})

export const {} = userSlice.actions
export const selectUser = (state: RootState) => state.user
export default userSlice.reducer
