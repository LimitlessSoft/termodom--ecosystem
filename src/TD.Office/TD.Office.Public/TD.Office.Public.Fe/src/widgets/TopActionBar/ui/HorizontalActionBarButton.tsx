import { Button, Grid, styled } from "@mui/material"
import { ReactNode } from "react"

interface IHorizontalActionBarButtonProps {
    text: string,
    onClick: () => void
}

export const HorizontalActionBarButton = (props: IHorizontalActionBarButtonProps): JSX.Element => {

    const HorizontalActionBarButtonStyled = styled(Button)(
        ({ theme }) => `
            margin: 0 0.5rem;
        `
    );

    return (
        <HorizontalActionBarButtonStyled
            variant={`contained`}
            onClick={() => {
                props.onClick()
            }}>
            {props.text}
        </HorizontalActionBarButtonStyled>
    )
}