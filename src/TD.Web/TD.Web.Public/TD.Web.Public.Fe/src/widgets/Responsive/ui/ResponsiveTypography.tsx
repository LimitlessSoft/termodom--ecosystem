import { Typography, styled } from '@mui/material'

export const ResponsiveTypography = styled(Typography)<{
    component?: any
    scale?: number
}>(
    ({ theme, scale }) =>
        `
@media only
    screen and (max-width: 1920px),
    {
        font-size: calc(${theme.fontSizes?._1920} * ${scale ?? 1});
    }

@media only
    screen and (max-width: 1600px),
    {
        font-size: calc(${theme.fontSizes?._1600} * ${scale ?? 1});
    }

@media only
    screen and (max-width: 1336px),
    {
        font-size: calc(${theme.fontSizes?._1336} * ${scale ?? 1});
    }

@media only
    screen and (max-width: 960px),
    {
        font-size: calc(${theme.fontSizes?._960} * ${scale ?? 1});
    }

@media only
    screen and (max-width: 720px),
    {
        font-size: calc(${theme.fontSizes?._720} * ${scale ?? 1});
    }

@media only
    screen and (max-width: 520px),
    {
        font-size: calc(${theme.fontSizes?._520} * ${scale ?? 1});
    }

@media only
    screen and (max-width: 360px),
    {
        font-size: calc(${theme.fontSizes?._360} * ${scale ?? 1});
    }
    
@media only
    screen and (max-width: 260px),
    {
        font-size: calc(${theme.fontSizes?._260} * ${scale ?? 1});
    }
`
)
