import { Button, styled } from '@mui/material'

export const HorizontalActionBarButton = (props) => {
    const HorizontalActionBarButtonStyled = styled(Button)(
        ({ theme }) => `
            margin: 0 0.5rem;
        `
    )

    return (
        <HorizontalActionBarButtonStyled variant="contained" {...props}>
            {props.text || props.children}
        </HorizontalActionBarButtonStyled>
    )
}
