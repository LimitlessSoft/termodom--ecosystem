import { TableRow, styled } from '@mui/material'

export const KorisniciListRowStyled = styled(TableRow)<{ user: any }>(
    ({ theme, user }) =>
        `
        transition-duration: 0.1s;
        background-color: ${user.isActive ? 'initial' : '#eee'};

        &:hover {
            cursor: pointer;
            background-color: ${theme.palette.secondary.light};
        }
    `
)
