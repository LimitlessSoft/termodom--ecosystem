import { ResponsiveTypography } from "@/widgets/Responsive";
import { Typography, styled } from "@mui/material";

export const ProizvodiListItemTitleStyled = styled(ResponsiveTypography)<{ }>
(({ theme }) => (
    `
        font-family: GothamProMedium;
        color: ${theme.palette.text.primary};
        text-align: center;
    `
))