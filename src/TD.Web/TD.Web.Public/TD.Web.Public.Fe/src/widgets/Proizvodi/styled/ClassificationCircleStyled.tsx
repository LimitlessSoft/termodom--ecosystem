import { getClassificationColor } from "@/app/helpers/proizvodiHelpers"
import { Grid, styled } from "@mui/material"

const size = '25px'
const boxShadow = "0px 0px 2px 1px rgba(0,0,0,0.8)"

export const ClassificationCircleStyled = styled(Grid)<{classification: number}>(
    ({theme, classification}) => `
    position: absolute;
    -webkit-box-shadow: ${boxShadow};
    -moz-box-shadow: ${boxShadow};
    box-shadow: ${boxShadow};
    border: 1px solid #444;
    background-color: ${getClassificationColor(classification)};
    top: 10px;
    right: 10px;
    z-index: 200;
    width: ${size};
    height: ${size};
    border-radius: 50%;
    `
)