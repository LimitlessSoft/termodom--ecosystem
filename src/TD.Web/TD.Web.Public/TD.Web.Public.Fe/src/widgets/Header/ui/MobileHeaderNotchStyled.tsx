import { Grid, styled } from "@mui/material";

export const MobileHeaderNotchStyled = styled(Grid)(
    ({ theme }) => `
        background-color: var(--td-red);
        top: 0;
        left: 0;
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
    `
)