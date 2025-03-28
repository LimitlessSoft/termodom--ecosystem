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
            font-size: 0.875rem;
            font-weight: 600;
            &:first-child {
                margin-left: 0;
            }
            @media only screen and (max-width: ${theme.breakpoints.values.sm}px) {
               font-size: 0.7rem;
            }
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
