import { configureStore } from "@reduxjs/toolkit";
import counterSliceReducer from './../features/slices/counterSlice'
import userSliceReducer from './../features/userSlice/userSlice'

export const store = configureStore({
    reducer: {
        counter: counterSliceReducer,
        user: userSliceReducer
    }
})

export type RootState = ReturnType<typeof store.getState>

export type AppDispatch = typeof store.dispatch