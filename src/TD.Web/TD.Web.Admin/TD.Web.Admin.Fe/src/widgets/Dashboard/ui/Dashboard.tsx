import { Grid } from "@mui/material"
import { DashboardProductsViewsPanel } from "./DashboardProductsViewsPanel"

export const Dashboard = (): JSX.Element => {
    return (
        <Grid p={2} container>
            <DashboardProductsViewsPanel />
        </Grid>
    )
}