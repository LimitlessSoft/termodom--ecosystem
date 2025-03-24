import { Button, Grid, styled } from '@mui/material'
import { ReactNode } from 'react'

interface IHorizontalActionBarButtonProps {
    text: string
    onClick: () => void
    isDisabled?: boolean
    startIcon?: ReactNode
    color?: string
}

export const HorizontalActionBarButton = (
    props: IHorizontalActionBarButtonProps
): JSX.Element => {
    const HorizontalActionBarButtonStyled = styled(Button)(
        ({ theme }) => `
            margin: 0.2rem 0.2rem;
        `
    )

    return (
        <HorizontalActionBarButtonStyled
            color={props.color || `primary`}
            startIcon={props.startIcon}
            disabled={props.isDisabled}
            variant={`contained`}
            onClick={() => {
                props.onClick()
            }}
        >
            {props.text}
        </HorizontalActionBarButtonStyled>
    )
}
