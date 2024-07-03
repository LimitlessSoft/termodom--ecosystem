import { Box, styled } from "@mui/material";

export const XButtonStyled = styled(Box)(
    ({ theme }) => `
        display: none;

        @media only
            screen and (max-width: ${theme.breakpoints.values.md}px),
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
    `
)