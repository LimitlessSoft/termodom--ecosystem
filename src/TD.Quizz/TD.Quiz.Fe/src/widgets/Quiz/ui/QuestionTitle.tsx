import { Grid, Typography } from "@mui/material"
import { IQuestionTitleProps } from "../types/IQuestionTitleProps"

export const QuestionTitle = (props: IQuestionTitleProps): JSX.Element => {
    return (
        <Grid item xs={8}>
            <Typography
                textAlign={'center'}
                variant={'h5'}
                p={1}>{ props.text }</Typography>
        </Grid>
    )
}