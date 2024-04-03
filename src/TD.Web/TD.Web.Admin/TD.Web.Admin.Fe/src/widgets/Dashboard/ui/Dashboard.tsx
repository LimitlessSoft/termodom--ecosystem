import { Grid } from "@mui/material"
import { DashboardProductsViewsPanel } from "./DashboardProductsViewsPanel"
import { DashboardSearchPhrasesPanel } from "./DashboardSearchPhrasesPanel"

export const Dashboard = (): JSX.Element => {
    return (
        <Grid p={2} container spacing={2}>
            <DashboardSearchPhrasesPanel />
            <DashboardProductsViewsPanel />
        </Grid>
    )
}