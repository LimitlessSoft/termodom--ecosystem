import { Grid, Typography, styled } from "@mui/material"

export const KolicinaInputFieldButton = (props: any): JSX.Element => {
    return (
        <KolicinaInputFieldButtonStyled item sm={6} container direction={`column`} justifyContent={`center`}>
            <KolicinaInputFiledButtonInnerStyled>{props.text}</KolicinaInputFiledButtonInnerStyled>
        </KolicinaInputFieldButtonStyled>
    )
}

const KolicinaInputFieldButtonStyled = styled(Grid)(
    ({ theme }) => `
        background-color: ${theme.palette.primary.contrastText};
        border: 1px solid gray;
        transition-duration: 0.1s;

        &:hover {
            cursor: pointer;
            background-color: ${theme.palette.primary.main};
            border-color: ${theme.palette.primary.main};
            color: ${theme.palette.primary.contrastText};
        }
    `
)

const KolicinaInputFiledButtonInnerStyled = styled(Typography)(
    ({ theme }) => `
        font-weight: bold;
        user-select: none;
    `
)