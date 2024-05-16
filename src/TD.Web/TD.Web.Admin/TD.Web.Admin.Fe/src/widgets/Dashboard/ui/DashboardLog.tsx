import "ls-release-log/dist/index.css"
import { LSReleaseLog } from "ls-release-log"
import { DashboardPanel } from "./DashboardPanel"
import { Typography } from "@mui/material"

export const DashboardLog = (): JSX.Element => {
    return (
        <DashboardPanel>
            <Typography>Release notes</Typography>
            <LSReleaseLog githubBearerToken="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" repoOwner="LimitlessSoft" repoName="termodom--ecosystem"/>
        </DashboardPanel>
    )
}