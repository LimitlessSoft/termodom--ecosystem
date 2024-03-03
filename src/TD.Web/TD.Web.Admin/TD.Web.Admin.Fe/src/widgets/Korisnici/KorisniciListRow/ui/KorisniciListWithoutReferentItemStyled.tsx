import { Typography, styled } from "@mui/material";

export const KorisniciListWithoutReferentItemStyled = styled(Typography)(
({ theme }) =>
`
    padding: 5px;
    background-color: ${theme.palette.primary.main};
    color: ${theme.palette.primary.contrastText};

    &:hover {
        cursor: pointer;
        background-color: ${theme.palette.primary.light};
    }
`)