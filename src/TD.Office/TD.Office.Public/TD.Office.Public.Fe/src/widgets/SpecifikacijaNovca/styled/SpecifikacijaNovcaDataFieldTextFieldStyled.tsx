import { styled, TextField } from '@mui/material'

const primaryColor = `#444`

export const SpecifikacijaNovcaDataFieldTextFieldStyled = styled(TextField)<{
    readonly?: boolean
}>(
    ({ theme, readonly }) => `
        .Mui-disabled {
            input {
                color: ${primaryColor};
                background-color: ${theme.palette.grey[200]};
                -webkit-text-fill-color: ${primaryColor};
            }
            ${readonly && `color: ${primaryColor};`}
        }
        
        [class*='notchedOutline'] {
            ${readonly && `border-color: ${primaryColor};`}
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
