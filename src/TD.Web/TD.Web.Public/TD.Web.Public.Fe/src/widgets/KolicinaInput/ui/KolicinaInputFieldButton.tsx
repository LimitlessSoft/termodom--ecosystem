import { Grid, Typography, styled } from '@mui/material'

export const KolicinaInputFieldButton = (props: any): JSX.Element => {
    return (
        <KolicinaInputFieldButtonStyled
            disabled={props.disabled}
            item
            sm={6}
            container
            direction={`column`}
            justifyContent={`center`}
            onClick={() => {
                if (props.disabled) return
                props.onClick()
            }}
        >
            <KolicinaInputFiledButtonInnerStyled>
                {props.text}
            </KolicinaInputFiledButtonInnerStyled>
        </KolicinaInputFieldButtonStyled>
    )
}

const KolicinaInputFieldButtonStyled = styled(Grid)<{ disabled: boolean }>(
    ({ theme, disabled }) => `
        background-color: ${theme.palette.primary.contrastText};
        border: 1px solid gray;
        transition-duration: 0.1s;
        height: 50%;
        -ms-user-select: none;
        -webkit-user-select: none;
        user-select: none;

        ${
            disabled
                ? `color: gray;`
                : `&:hover {
                    cursor: pointer;
                    background-color: ${theme.palette.primary.main};
                    border-color: ${theme.palette.primary.main};
                    color: ${theme.palette.primary.contrastText};
                }`
        }

        @media only
            screen and (max-width: 260px),
            screen and (max-width: 360px),
            screen and (max-width: 520px),
            {

            }
    `
)

const KolicinaInputFiledButtonInnerStyled = styled(Typography)(
    ({ theme }) => `
        font-weight: bold;
        user-select: none;
    `
)
