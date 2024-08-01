import { Grid, styled } from '@mui/material'

export const UserPriceLabelStyled = styled(Grid)<{ component?: any }>(
    ({ theme }) =>
        `
    flex-grow: 0;
    max-width: 50%;
    flex-basis: 50%;

    @media only
    screen and (max-width: 260px),
    screen and (max-width: 360px),
    screen and (max-width: 520px),
    screen and (max-width: 720px),
    screen and (max-width: 960px),
    {
        flex-basis: 100%;
        max-width: 100%;
        margin-bottom: 30px;

        h2 {
            border: none;
        }
    }
`
)
