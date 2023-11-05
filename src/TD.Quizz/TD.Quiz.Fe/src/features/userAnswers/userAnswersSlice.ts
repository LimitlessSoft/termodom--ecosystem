import { PayloadAction, createSlice } from "@reduxjs/toolkit";

interface UserAnswersState {
    correctAnswers: number,
    incorrectAnswers: number
}

const initialState: UserAnswersState = {
    correctAnswers: 0,
    incorrectAnswers: 0
}

export const userAnswersSlice = createSlice({
    name: 'usersAnswers',
    initialState,
    reducers: {
        increaseCorrectAnswers: (state, action: PayloadAction<number>) => {
            state.correctAnswers += action.payload
        },
        increaseIncorrectAnswers: (state, action: PayloadAction<number>) => {
            state.incorrectAnswers += action.payload
        }
    }
})

export const { increaseCorrectAnswers, increaseIncorrectAnswers } = userAnswersSlice.actions

export default userAnswersSlice.reducer