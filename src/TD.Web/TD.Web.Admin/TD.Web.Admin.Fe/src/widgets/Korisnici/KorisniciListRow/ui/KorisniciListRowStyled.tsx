import { TableRow, styled } from "@mui/material";

export const KorisniciListRowStyled = styled(TableRow)<{ isActive: boolean }>(
({ theme, isActive }) =>
    `
        transition-duration: 0.1s;
        background-color: ${ isActive ? 'initial' : '#eee' };

        &:hover {
            cursor: pointer;
            background-color: ${theme.palette.secondary.light};
        }
    `
)