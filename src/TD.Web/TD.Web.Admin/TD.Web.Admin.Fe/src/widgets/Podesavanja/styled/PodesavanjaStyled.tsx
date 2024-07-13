import {Box, styled} from "@mui/material";

export const PodesavanjaStyled = styled(Box)(
    ({ theme }) => `
        position: fixed;
        left: 0;
        z-index: -1;
        height: 100vh;
        width: 100%;
        overflow: hidden;
    `
)