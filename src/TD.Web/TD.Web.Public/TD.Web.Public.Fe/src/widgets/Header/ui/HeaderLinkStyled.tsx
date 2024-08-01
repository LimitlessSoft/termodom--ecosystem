import { styled } from '@mui/material'
import Link from 'next/link'

export const HeaderLinkStyled = styled(Link)<{ component: any }>(
    ({ theme }) => `
        text-decoration: none;
        color: var(--td-white);
        padding-top: 20px;
        padding-bottom: 20px;
        padding-left: 15px;
        padding-right: 15px;

        @media only
            screen and (max-width: 260px),
            screen and (max-width: 360px),
            screen and (max-width: 520px),
            screen and (max-width: 720px),
        {
            width: 100%;
            padding: 20px 0;
            text-align: center;
        }
    `
)
