import { Grid, styled } from '@mui/material'

export const ProizvodiListItemStyled = styled(Grid)(
    ({ theme }) => `
        width: calc((100% - 80px) / 5);
        margin: 8px;
        position: relative;

        @media only screen and (max-width: 960px) {
            width: calc(calc(100% / 4) - calc(8px * 4));
        }

        @media only screen and (max-width: 720px) {
            width: calc(33% - calc(8px * 3));
            width: calc(calc(100% / 3) - calc(8px * 3));
        }

        @media only screen and (max-width: 520px) {
            width: calc(calc(100% / 2) - calc(8px * 2));
        }
    `
)
