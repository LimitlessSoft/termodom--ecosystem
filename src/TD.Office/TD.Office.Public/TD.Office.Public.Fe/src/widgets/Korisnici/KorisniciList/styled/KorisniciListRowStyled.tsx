import { TableRow, styled } from '@mui/material'

export const KorisniciListRowStyled = styled(TableRow)(
    ({ theme }) => `
        &:nth-of-type(odd) {
            background-color: ${theme.palette.grey[200]};
        }

        &:hover {
            background-color: ${theme.palette.action.hover};
            cursor: pointer;
        }
    `
)
