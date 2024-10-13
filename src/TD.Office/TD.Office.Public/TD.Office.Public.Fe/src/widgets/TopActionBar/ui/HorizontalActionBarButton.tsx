import { Button, Grid, styled } from '@mui/material'
import { ReactNode } from 'react'

interface IHorizontalActionBarButtonProps {
    text: string
    onClick: () => void
    disabled?: boolean
    startIcon?: ReactNode
    color?: 'primary' | 'secondary'
}

export const HorizontalActionBarButton = (
    props: IHorizontalActionBarButtonProps
) => {
    const HorizontalActionBarButtonStyled = styled(Button)(
        ({ theme }) => `
            margin: 0 0.5rem;
        `
    )

    return (
        <HorizontalActionBarButtonStyled
            startIcon={props.startIcon}
            disabled={props.disabled}
            color={props.color}
            variant={`contained`}
            onClick={() => {
                props.onClick()
            }}
        >
            {props.text}
        </HorizontalActionBarButtonStyled>
    )
}
