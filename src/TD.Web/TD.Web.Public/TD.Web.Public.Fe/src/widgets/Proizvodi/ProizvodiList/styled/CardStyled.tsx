import { getClassificationColor } from "@/app/helpers/proizvodiHelpers"
import { Card, styled } from "@mui/material"

export const CardStyled = styled(Card)<{classification: number}>(
    ({ theme, classification }) => `
        border: 4px solid;
        width: calc(100% - 8px);
        border-color: ${getClassificationColor(classification)};

        img {
            max-height: 170px;
            height: 50vw;
        }

        @media only screen and (max-width: 260px) {
        }
    `)