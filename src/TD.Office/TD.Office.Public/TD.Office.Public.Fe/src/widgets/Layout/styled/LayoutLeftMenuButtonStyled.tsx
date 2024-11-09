import { Grid, styled } from '@mui/material'

export const LayoutLeftMenuButtonStyled = styled(Grid)(
    ({ theme }) => `
        display: flex;
        align-items: center;
        gap: 1rem;
        padding: 0.5rem 1rem;
        
        &:hover {
            background-color: ${theme.palette.primary.dark};
            cursor: pointer;
        }
    `
)
