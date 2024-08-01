import { Paper, styled } from '@mui/material'

export const DashboardPanelStyled = styled(Paper)(
    ({ theme }) => `
        padding: ${theme.spacing(2)};
    `
)
