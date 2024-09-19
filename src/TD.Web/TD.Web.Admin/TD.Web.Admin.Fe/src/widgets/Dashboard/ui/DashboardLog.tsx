import 'ls-release-log/dist/index.css'
import { LSReleaseLog } from 'ls-release-log'
import { DashboardPanel } from './DashboardPanel'
import { Typography } from '@mui/material'

export const DashboardLog = (): JSX.Element => {
    return (
        <DashboardPanel>
            <Typography>Release notes</Typography>
            <LSReleaseLog
                githubBearerToken="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"
                repoOwner="LimitlessSoft"
                repoName="termodom--ecosystem"
            />
        </DashboardPanel>
    )
}
