import { TextField, styled } from "@mui/material"

const primaryColor = `#444`

export const EnchantedTextFieldStyled = styled(TextField)<{
    readOnly?: boolean,
    textalignment?: 'left' | 'center' | 'right' | undefined
}>(
    ({ theme, readOnly, textalignment }) => `
        
        input {
            text-align: ${ textalignment ?? 'right' };
        }
        
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