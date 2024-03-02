import { TableRow, styled } from "@mui/material";

export const KorisniciListRowStyled = styled(TableRow)(
({ theme }) =>
    `
        transition-duration: 0.1s;
        &:hover {
            cursor: pointer;
            background-color: ${theme.palette.secondary.light};
        }
    `
)