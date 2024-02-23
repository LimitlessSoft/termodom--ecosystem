import { Grid, styled } from "@mui/material";

export const MobileHeaderNotchStyled = styled(Grid)(
    ({ theme }) => `
        display: none;
        background-color: var(--td-red);
        top: 0;
        left: 0;
        width: 100vw;
        padding: 8px;
        z-index: 100;

        span {
            display: block;
            width: 50px;
            border-radius: 4px;
            height: 10px;
            margin: 5px;
            background-color: #222;
        }

        @media only
            screen and (max-width: 260px),
            screen and (max-width: 360px),
            screen and (max-width: 520px),
            screen and (max-width: 720px),
        {
            display: block;
        }
    `
)