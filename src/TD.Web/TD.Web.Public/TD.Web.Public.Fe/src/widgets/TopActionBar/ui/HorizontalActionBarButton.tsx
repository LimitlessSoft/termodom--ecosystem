import { Button, Grid, styled } from '@mui/material'
import { ReactNode } from 'react'

interface IHorizontalActionBarButtonProps {
    text: string
    onClick: () => void
    disabled?: boolean
    startIcon?: ReactNode
}

export const HorizontalActionBarButton = (
    props: IHorizontalActionBarButtonProps
): JSX.Element => {
    const HorizontalActionBarButtonStyled = styled(Button)(
        ({ theme }) => `
            margin: 0 0.5rem;
        `
    )

    return (
        <HorizontalActionBarButtonStyled
            startIcon={props.startIcon}
            disabled={props.disabled}
            variant={`contained`}
            onClick={() => {
                props.onClick()
            }}
        >
            {props.text}
        </HorizontalActionBarButtonStyled>
    )
}
