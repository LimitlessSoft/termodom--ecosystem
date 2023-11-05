import userAnswersSlice from "@/features/userAnswers/userAnswersSlice";
import { configureStore } from "@reduxjs/toolkit";

export const store = configureStore({
    reducer: {
        userAnswers: userAnswersSlice
    }
})

export type RootState = ReturnType<typeof store.getState>

export const selectCorrectAnswersCount = (state: RootState) => state.userAnswers.correctAnswers
export const selectIncorrectAnswersCount = (state: RootState) => state.userAnswers.incorrectAnswers

export type AppDispatch = typeof store.dispatch