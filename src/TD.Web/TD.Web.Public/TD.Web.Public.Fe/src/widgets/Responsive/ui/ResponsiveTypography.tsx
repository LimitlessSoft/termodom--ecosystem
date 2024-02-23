import { Typography, styled } from "@mui/material";

export const ResponsiveTypography = styled(Typography)<{ component?: any }>
(({ theme }) =>
`
@media only
    screen and (max-width: 1920px),
    {
        font-size: ${theme.fontSizes?._1920};
    }

@media only
    screen and (max-width: 1600px),
    {
        font-size: ${theme.fontSizes?._1600};
    }

@media only
    screen and (max-width: 1336px),
    {
        font-size: ${theme.fontSizes?._1336};
    }

@media only
    screen and (max-width: 960px),
    {
        font-size: ${theme.fontSizes?._960};
    }

@media only
    screen and (max-width: 720px),
    {
        font-size: ${theme.fontSizes?._720};
    }

@media only
    screen and (max-width: 520px),
    {
        font-size: ${theme.fontSizes?._520};
    }

@media only
    screen and (max-width: 360px),
    {
        font-size: ${theme.fontSizes?._360};
    }
    
@media only
    screen and (max-width: 260px),
    {
        font-size: ${theme.fontSizes?._260};
    }
`)