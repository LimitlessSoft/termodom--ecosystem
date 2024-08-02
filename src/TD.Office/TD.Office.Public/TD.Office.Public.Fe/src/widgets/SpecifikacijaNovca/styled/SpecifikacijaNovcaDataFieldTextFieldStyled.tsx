import { styled, TextField } from '@mui/material'

const primaryColor = `#444`

export const SpecifikacijaNovcaDataFieldTextFieldStyled = styled(TextField)<{
    readOnly?: boolean
}>(
    ({ theme, readOnly }) => `
        .Mui-disabled {
            input {
                color: ${primaryColor};
                background-color: ${theme.palette.grey[200]};
                -webkit-text-fill-color: ${primaryColor};
            }
            ${readOnly && `color: ${primaryColor};`}
        }
        
        [class*='notchedOutline'] {
            ${readOnly && `border-color: ${primaryColor};`}
        }
        
        @media only screen and (min-width: ${theme.breakpoints.values.sm}px)
        {
            input {
                min-width: 200px;
            }
        }
        
        @media only screen and (min-width: ${theme.breakpoints.values.lg}px)
        {
            input {
                min-width: 300px;
            }
        }
        
    `
)
