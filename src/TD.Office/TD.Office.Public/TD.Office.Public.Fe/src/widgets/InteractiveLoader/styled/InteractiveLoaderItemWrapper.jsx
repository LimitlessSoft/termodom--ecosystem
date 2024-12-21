import { Paper, styled } from '@mui/material'

export const InteractiveLoaderItemWrapper = styled(Paper)(
    ({ theme }) => `
        display: flex;
        align-items: center;
        justify-content: center;
        min-width: 300px;
        padding: ${theme.spacing(2)};
        background-color: ${theme.palette.secondary.main};
        color: ${theme.palette.secondary.contrastText};
    `
)
