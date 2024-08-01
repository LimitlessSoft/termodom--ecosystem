import { Box, styled } from '@mui/material'

export const SpecifikacijaNovcaOstaloCommentFieldStyled = styled(Box)<{
    readOnly?: boolean
}>(
    ({ theme, readOnly }) => `
        position: absolute;
        background-color: #fff;
        top: -45px;
        right: 0;
    `
)
