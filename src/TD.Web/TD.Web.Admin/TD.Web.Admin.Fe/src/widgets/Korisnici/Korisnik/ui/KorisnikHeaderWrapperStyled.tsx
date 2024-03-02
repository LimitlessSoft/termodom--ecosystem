import { Grid, styled } from "@mui/material";

export const KorisnikHeaderWrapperStyled = styled(Grid)(
    ({ theme }) =>`
        justify-content: center;
        border: 1px solid ${theme.palette.primary.main};
        padding: 20px;
        border-radius: 10px;

        > div {
            margin: 0 10px;
        }
    `)