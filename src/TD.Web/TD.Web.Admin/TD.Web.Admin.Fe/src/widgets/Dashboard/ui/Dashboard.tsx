import { DashboardProductsViewsPanel } from './DashboardProductsViewsPanel'
import { DashboardSearchPhrasesPanel } from './DashboardSearchPhrasesPanel'
import { DashboardLog } from './DashboardLog'
import { Grid } from '@mui/material'

export const Dashboard = (): JSX.Element => {
    return (
        <Grid p={2} container spacing={2}>
            <DashboardLog />
            <DashboardSearchPhrasesPanel />
            <DashboardProductsViewsPanel />
        </Grid>
    )
}
