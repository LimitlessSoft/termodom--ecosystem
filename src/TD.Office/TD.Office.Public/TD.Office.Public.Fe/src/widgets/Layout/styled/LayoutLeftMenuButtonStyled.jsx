import { Grid, styled } from '@mui/material'

export const LayoutLeftMenuButtonStyled = styled(Grid)(
    ({ theme }) => `
        display: flex;
        align-items: center;
        gap: 1rem;
        padding: 0.5rem 1rem;
        width: 100%;
        
        @media screen and (min-width: ${theme.breakpoints.values.md}px) {
            &:hover {
                background-color: ${theme.palette.primary.dark};
                cursor: pointer;
            }
        }

        @media screen and (max-width: ${theme.breakpoints.values.md}px) {
            justify-content: center;
            
            svg, p {
                font-size: 1.3rem;
            }
        }
    `
)
