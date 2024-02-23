import { Box, styled } from "@mui/material";

export const XButtonStyled = styled(Box)(
    ({ theme }) => `
        display: none;

        @media only
            screen and (max-width: 260px),
            screen and (max-width: 360px),
            screen and (max-width: 520px),
            screen and (max-width: 720px),
            {
                display: block;
                position: absolute;
                right: 5vw;
                top: 20px;
                background-color: rgba(0, 0, 0, 0.4);
                border-radius: 10px;
                padding: 10px 15px;
                color: white;
                font-size: 1.5rem;
        }

        @media only screen and (max-width: 960px) {
        }

        @media only screen and (max-width: 1336px) {
        }

        @media only screen and (max-width: 1600px) {
        }

        @media only screen and (max-width: 1920px) {
        }
    `
)