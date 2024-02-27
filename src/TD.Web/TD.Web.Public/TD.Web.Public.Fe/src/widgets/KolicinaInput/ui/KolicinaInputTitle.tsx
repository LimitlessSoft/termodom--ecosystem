import { Typography, styled } from "@mui/material";

const TITLE_BORDER_RADIUS = '5px'

export const KolicinaInputTitle = styled(Typography)(
    ({ theme }) => `
      color: ${theme.palette.primary.contrastText};
      font-weight: bold;
      text-align: center;
      padding: 5px;
      background-color: ${theme.palette.primary.main};
      border-radius: ${TITLE_BORDER_RADIUS} ${TITLE_BORDER_RADIUS} 0px 0px;
      -ms-user-select: none;
      -webkit-user-select: none;
      user-select: none;
    `)