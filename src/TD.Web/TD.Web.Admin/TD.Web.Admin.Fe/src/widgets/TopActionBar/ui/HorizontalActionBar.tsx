import { Grid, styled } from '@mui/material'
import { ReactNode } from 'react'

interface IHorizontalActionBarProps {
    children: ReactNode
}

export const HorizontalActionBar = (
    props: IHorizontalActionBarProps
): JSX.Element => {
    const HorizontalActionBarStyled = styled(Grid)(
        ({ theme }) => `
            padding: 1rem;
        `
    )

    return (
        <HorizontalActionBarStyled container>
            {props.children}
        </HorizontalActionBarStyled>
    )
}
