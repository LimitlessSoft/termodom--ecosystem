import { Grid, styled } from "@mui/material";

export const LayoutLeftMenuButtonStyled = styled(Grid)(
    ({ theme }) => `
        transition-duration: 0.3s;

        svg {
            padding: 0.5rem 1rem;
        }

        &:hover {
            background-color: ${theme.palette.primary.dark};
            cursor: pointer;
        }
    `
)