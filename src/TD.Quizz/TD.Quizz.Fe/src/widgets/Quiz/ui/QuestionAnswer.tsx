import { Button, Grid, Typography } from "@mui/material"
import { IQuestionAnswerProps } from "../types/IQuestionAnswerProps"

export const QuestionAnswer = (props: IQuestionAnswerProps): JSX.Element => {
    return (
        <Grid item xs={6} p={1} textAlign={'center'}>
            <Button variant={'contained'}>
                <Typography
                    textAlign={'center'}
                    variant={'body2'}
                    p={1}>{ props.text }</Typography>
            </Button>
        </Grid>
    )
}